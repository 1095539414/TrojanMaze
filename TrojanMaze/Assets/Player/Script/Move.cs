using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Move : MonoBehaviour, iDamageable {
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] public float speed = 10f;
    [SerializeField] GameObject swordPivot;
    [SerializeField] GameObject gunPivot;
    [SerializeField] GameObject playerBullet;
    [SerializeField] GameObject holdingProgressR;
    [SerializeField] GameObject holdingProgressT;
    [SerializeField] GameObject weaponChangeUI;

    [SerializeField] GameObject FootPrintLeft;
    [SerializeField] GameObject FootPrintRight;

    [SerializeField] GameObject gunItem;
    [SerializeField] GameObject swordItem;

    [SerializeField] float FootprintGap = 1f;
    private bool _footPrintLeft;

    // Portal-related variables
    [SerializeField] GameObject Portal;
    Vector2 moveInput;
    Rigidbody2D _body;

    public Animator animator;
    const float MAX_HP = 1f;
    static float HP;
    private float nextShootTime;
    private int bulletNum = 0;
    private GameObject gun;
    public GameObject SpeedIncrease_Effect;
    public GameObject MedicalKit_Effect;
    public GameObject BoostView_Effect;
    public GameObject Trap_Effect;

    public static float totalHpReduced = 0f;
    public static float dmgBySword = 0f;
    public static float dmgByGun = 0f;
    public static Move _move;
    public static float speedPotionTimer;
    public static float viewPotionTimer;

    // footprints-related variables
    private Vector2 _prevPos;
    private Vector2 _curPos;

    private GameObject _portal;
    private float _holdTimerTarget = 1f;
    private float _holdTimerT = 0f;
    private float _holdTimerR = 0f;
    private bool _isHoldingT = false;
    private bool _isHoldingR = false;

    private SpriteRenderer _renderer;
    private float _hurtTimer = 0f;
    public static Dictionary<string, float> damagedFrom = new Dictionary<string, float>();

    private bool _teleporting = false;
    private Renderer _holdingProgressRendererR;
    private Renderer _holdingProgressRendererT;
    private GameObject _weaponTouching;
    private float increasedSpeed;

    private void Awake() {
        _move = this;//static this scirpts for other scripts to deploy
    }

    void Start() {
        increasedSpeed = 1.5f * speed;
        _body = GetComponent<Rigidbody2D>();
        swordPivot.SetActive(false);
        gunPivot.SetActive(false);
        HP = MAX_HP;
        GameManager.instance.DeathUI.SetActive(false);
        _prevPos = transform.position;
        _renderer = GetComponent<SpriteRenderer>();
        _holdingProgressRendererR = holdingProgressR.GetComponent<Renderer>();
        _holdingProgressRendererT = holdingProgressT.GetComponent<Renderer>();
        _footPrintLeft = true;
        speedPotionTimer = 0f;
    }

    void Update() {
        fieldOfView.SetOrigin(transform.position);
        animator.SetFloat("Speed", Vector3.Magnitude(_body.velocity));
        if(_renderer.enabled) {
            DropFootprint();
            FlipPlayer();
        }
        // channeling the portal/etc.
        if(GameManager.instance.PortalObject.activeSelf && Input.GetKeyDown(KeyCode.R)) {
            if(_portal == null) {
                _portal = Instantiate(Portal, transform.position, transform.rotation);
            } else {
                _isHoldingR = true;
                holdingProgressR.SetActive(true);
            }
        }

        if(_portal != null && Input.GetKeyDown(KeyCode.T)) {
            _isHoldingT = true;
            holdingProgressT.SetActive(true);
        }

        // reset the channeling
        if(Input.GetKeyUp(KeyCode.T)) {
            _isHoldingT = false;
            _holdTimerT = 0f;
        }

        if(Input.GetKeyUp(KeyCode.R)) {
            _isHoldingR = false;
            _holdTimerR = 0f;
        }

        if(_isHoldingR && !_isHoldingT) {
            _holdingProgressRendererR.sharedMaterial.SetFloat(
                "_Arc2", 360f - _holdTimerR / _holdTimerTarget * 360f
            );
            _holdTimerR += Time.deltaTime;
            if(_holdTimerR >= _holdTimerTarget) {
                _holdTimerR = 0f;
                _isHoldingR = false;
                Destroy(_portal);
                _portal = null;
            }
        } else {
            holdingProgressR.SetActive(false);
            _holdingProgressRendererR.sharedMaterial.SetFloat("_Arc2", 360f);
        }

        if(!_isHoldingR && _isHoldingT) {
            _holdingProgressRendererT.sharedMaterial.SetFloat(
                "_Arc2", 360f - _holdTimerT / _holdTimerTarget * 360f
            );

            _holdTimerT += Time.deltaTime;
            _body.velocity = Vector3.zero;
            if(_holdTimerT >= _holdTimerTarget) {
                _holdTimerT = -100f;
                StartCoroutine(TeleportBack());
            }
        } else {
            holdingProgressT.SetActive(false);
            _holdingProgressRendererT.sharedMaterial.SetFloat("_Arc2", 360f);
        }

        if(_portal == null) {
            GameManager.instance.PortalImage.color = Color.white;
        } else {
            Color c = Color.gray;
            c.a = 0.7f;
            GameManager.instance.PortalImage.color = c;
        }

        GameManager.instance.PortalT.enabled = _portal != null;
        GameManager.instance.PortalR.enabled = _portal == null;
        GameManager.instance.PortalDisgard.SetActive(_portal != null);

        DealWithWeapon();
        if(gunPivot.activeSelf) {
            if(bulletNum > 0 && (Input.GetMouseButton(0) || Input.GetKeyDown("space")) && Time.time >= nextShootTime) {
                nextShootTime = Time.time + 0.6f;
                SoundManager.PlaySound("gunshot");
                Instantiate(playerBullet, this.transform.position, this.transform.rotation);
                StartCoroutine(UpdateBulletNumUI(bulletNum - 1));
            }
        }

        if(_hurtTimer > 0) {
            _hurtTimer -= Time.deltaTime;
            if(_hurtTimer < 0) {
                StartCoroutine(StopHurtAnimation());
                _hurtTimer = 0f;
            }
        }
        if(speedPotionTimer > 0) {
            speedPotionTimer -= Time.deltaTime;
        } else {
            speedPotionTimer = 0f;
        }

        if(viewPotionTimer > 0) {
            viewPotionTimer -= Time.deltaTime;
            FieldOfView.BoostViewDistance();
        } else {
            viewPotionTimer = 0f;
            FieldOfView.ResetViewDistance();
        }
    }

    IEnumerator TeleportBack() {
        float elapsedTime = 0f;
        float waitTime = 0.08f;
        Vector3 originalScale = transform.localScale;
        Vector3 finalScale = new Vector3(0.05f, originalScale.y * 1.5f, originalScale.z);
        while(elapsedTime <= waitTime) {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, finalScale, elapsedTime / waitTime);
            yield return null;
        }
        _renderer.enabled = false;
        holdingProgressT.SetActive(false);
        fieldOfView.enabled = false;
        bool swordStatus = swordPivot.activeSelf;
        swordPivot.SetActive(false);
        bool gunStatus = gunPivot.activeSelf;
        gunPivot.SetActive(false);
        _teleporting = true;

        yield return new WaitForSeconds(0.2f);

        Vector3 originalLocation = transform.position;
        Vector3 finalLocation = _portal.GetComponent<Portal>().GetLocation();

        elapsedTime = 0f;
        waitTime = 0.5f;
        while(elapsedTime <= waitTime) {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(originalLocation, finalLocation, elapsedTime / waitTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        fieldOfView.enabled = true;
        _renderer.enabled = true;
        _teleporting = false;
        swordPivot.SetActive(swordStatus);
        gunPivot.SetActive(gunStatus);

        elapsedTime = 0f;
        waitTime = 0.08f;
        while(elapsedTime <= waitTime) {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(finalScale, originalScale, elapsedTime / waitTime);
            yield return null;
        }
        Destroy(_portal);
        _portal = null;
        _isHoldingT = false;
        _holdTimerT = 0f;
    }

    void FixedUpdate() {
        if(!_isHoldingT) {
            Run();
        }
    }


    void Run() {
        Vector2 moveSpeed = new Vector2(moveInput.x * speed, moveInput.y * speed) * Time.deltaTime;
        if(speedPotionTimer > 0f) {
            moveSpeed = new Vector2(moveInput.x * increasedSpeed, moveInput.y * increasedSpeed) * Time.deltaTime;
        }
        _body.velocity = moveSpeed;
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    void FlipPlayer() {
        if(swordPivot.activeSelf || gunPivot.activeSelf) {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            Vector3 diff = GameManager.instance.MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.localScale = new Vector2(
                Mathf.Sign(diff.x) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y
            );
        } else {
            bool hasHorizontalSpeed = Mathf.Abs(_body.velocity.x) > Mathf.Epsilon;
            if(hasHorizontalSpeed) {
                transform.localScale = new Vector2(Mathf.Sign(_body.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
        }

        if(transform.localScale.x <= 0 && holdingProgressT.transform.localScale.x >= 0 ||
            transform.localScale.x >= 0 && holdingProgressT.transform.localScale.x <= 0) {
            Vector3 scale = holdingProgressT.transform.localScale;
            scale.x *= -1;
            holdingProgressT.transform.localScale = scale;
            holdingProgressR.transform.localScale = scale;
            weaponChangeUI.transform.localScale = scale / 4f;
        }
    }

    public static void IncreaseHP(float value) {
        if(HP < MAX_HP) {
            HP += value;
        }
        if(HP > MAX_HP) {
            HP = MAX_HP;
        }
    }

    IEnumerator StartHurtAnimation() {
        float elapsedTime = 0f;
        float waitTime = 0.15f;
        Color originalColor = new Color(1, 1, 1);
        Color finalColor = new Color(1, 0, 0);
        while(elapsedTime <= waitTime) {
            elapsedTime += Time.deltaTime;
            _renderer.color = Color.Lerp(originalColor, finalColor, elapsedTime / waitTime);
            yield return null;
        }
    }


    IEnumerator StopHurtAnimation() {
        float elapsedTime = 0f;
        float waitTime = 0.15f;
        Color originalColor = new Color(1, 1, 1);
        Color finalColor = new Color(1, 0, 0);
        while(elapsedTime <= waitTime) {
            elapsedTime += Time.deltaTime;
            _renderer.color = Color.Lerp(finalColor, originalColor, elapsedTime / waitTime);
            yield return null;
        }
    }

    public bool ReduceHealth(float value, GameObject from) {
        if(HP > 0 && !_teleporting) {
            if(_hurtTimer == 0f) {
                StartCoroutine(StartHurtAnimation());
            }
            _hurtTimer = 0.15f;

            HP -= value;
            totalHpReduced += value;
            if(!damagedFrom.ContainsKey(from.name)) {
                damagedFrom.Add(from.name, 0f);
            }
            damagedFrom[from.name] += value;

            if(HP <= 0) {
                StartCoroutine(Die());
                UnityAnalytics.sendLevelDied();
                Time.timeScale = 0f;
                GameManager.instance.DeathUI.SetActive(true);
            }
            return true;
        }
        return false;
    }

    public static float GetHP() {
        return HP;
    }

    private void DropFootprint() {
        _curPos = transform.position;
        float dist = Mathf.Sqrt(Mathf.Pow(_curPos.x - _prevPos.x, 2) + Mathf.Pow(_curPos.y - _prevPos.y, 2));
        if(dist > FootprintGap) {
            float tanVal = (_curPos.y - _prevPos.y) / (_curPos.x - _prevPos.x);
            float rotationAngle = Mathf.Atan(tanVal) * (180 / Mathf.PI) + ((_curPos.x - _prevPos.x) < 0 ? 180 : 0);
            if(_footPrintLeft) {
                Instantiate(
                    FootPrintLeft,
                    transform.position - new Vector3(0, 0.45f, 0),
                    Quaternion.Euler(new Vector3(0, 0, rotationAngle))
                );
            } else {
                Instantiate(
                    FootPrintRight,
                    transform.position - new Vector3(0, 0.45f, 0),
                    Quaternion.Euler(new Vector3(0, 0, rotationAngle))
                );
            }
            _footPrintLeft = !_footPrintLeft;
            _prevPos = _curPos;
        }
    }

    public IEnumerator Die() {
        this.swordPivot.SetActive(false);
        this.gunPivot.SetActive(false);
        animator.SetBool("isDie", true);
        yield return new WaitForEndOfFrame();
    }

    public bool isTeleporting() {
        return _teleporting;
    }
    public void SpeedIncrease_Effectopen() {
        SpeedIncrease_Effect.SetActive(true);
        Invoke("SpeedIncrease_Effectclose", 2f);
    }

    void SpeedIncrease_Effectclose() {
        SpeedIncrease_Effect.SetActive(false);

    }

    public void MedicalKit_Effectopen() {
        MedicalKit_Effect.SetActive(true);


        Invoke("MedicalKit_Effectclose", 2f);

    }
    void MedicalKit_Effectclose() {
        MedicalKit_Effect.SetActive(false);
    }
    public void BoostView_Effectopen() {
        BoostView_Effect.SetActive(true);


        Invoke("BoostView_Effectclose", 2f);

    }
    void BoostView_Effectclose() {
        BoostView_Effect.SetActive(false);
    }

    public void WeaponTouched(GameObject weapon) {
        if(_weaponTouching != null) {
            _weaponTouching.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        _weaponTouching = weapon;

        if(!(swordPivot.activeSelf && weapon.CompareTag("sword"))) {
            _weaponTouching.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        }

    }
    public void WeaponUntouched(GameObject weapon) {
        if(_weaponTouching == weapon) {
            _weaponTouching.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            _weaponTouching = null;

        }
    }

    private IEnumerator UpdateBulletNumUI(int num) {
        Color color = Color.red;
        if(bulletNum <= num) {
            color = Color.green;
        }
        bulletNum = num;
        if(bulletNum > 0) {
            GameManager.instance.BulletImg.SetActive(true);
            GameManager.instance.BulletUI.text = bulletNum.ToString();
        } else {
            GameManager.instance.BulletImg.SetActive(false);
            GameManager.instance.BulletUI.text = "";
            this.gunPivot.SetActive(false);
        }
        float elapsedTime = 0f;
        float waitTime = 0.12f;
        while(elapsedTime < waitTime) {
            elapsedTime += Time.deltaTime;
            GameManager.instance.BulletUI.color = Vector4.Lerp(Color.white, color, elapsedTime / waitTime);
            yield return null;
        }
        elapsedTime = 0f;
        while(elapsedTime < waitTime) {
            elapsedTime += Time.deltaTime;
            GameManager.instance.BulletUI.color = Vector4.Lerp(color, Color.white, elapsedTime / waitTime);
            yield return null;
        }
    }
    private void PickupGun() {
        StartCoroutine(UpdateBulletNumUI(_weaponTouching.GetComponent<GunItem>().getBulletNum()));
        gunPivot.SetActive(true);
        Destroy(_weaponTouching);
        _weaponTouching = null;
    }
    private void PickupSword() {
        swordPivot.SetActive(true);
        Destroy(_weaponTouching);
        _weaponTouching = null;
    }

    private void DealWithWeapon() {
        if(!_teleporting && _weaponTouching != null) {
            if(!swordPivot.activeSelf && !gunPivot.activeSelf) {
                if(_weaponTouching.CompareTag("gun")) {
                    PickupGun();
                } else {
                    PickupSword();
                }
            } else if(Input.GetKeyDown(KeyCode.C)) {
                if(_weaponTouching.CompareTag("gun")) {
                    if(swordPivot.activeSelf) {
                        Instantiate(swordItem, _weaponTouching.transform.position, Quaternion.identity);
                        swordPivot.SetActive(false);
                        PickupGun();
                    } else if(gunPivot.activeSelf) {
                        StartCoroutine(
                            UpdateBulletNumUI(
                                _weaponTouching.GetComponent<GunItem>().UpdateBulletNum(bulletNum)
                            )
                        );
                    } else {
                        PickupGun();
                    }
                } else {
                    if(gunPivot.activeSelf) {
                        GameObject droppedGun = Instantiate(
                            gunItem,
                            _weaponTouching.transform.position,
                            Quaternion.identity
                        );
                        droppedGun.GetComponent<GunItem>().setBulletNum(bulletNum);
                        StartCoroutine(UpdateBulletNumUI(0));

                        PickupSword();
                    } else if(!swordPivot.activeSelf) {
                        PickupSword();
                    }
                }
            }
        }
        weaponChangeUI.SetActive(
            _weaponTouching != null &&
            !(_weaponTouching.CompareTag("sword") && swordPivot.activeSelf)
        );

    }

    protected void OnTriggerEnter2D(Collider2D other) {

    }

    private void OnTriggerExit(Collider other) {

    }
}
