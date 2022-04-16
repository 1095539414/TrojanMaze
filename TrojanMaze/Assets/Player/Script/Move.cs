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
    [SerializeField] float fireInterval = 1f;
    [SerializeField] GameObject swordPivot;
    [SerializeField] GameObject playerBullet;
    [SerializeField] GameObject holdingProgressR;
    [SerializeField] GameObject holdingProgressT;

    Vector2 moveInput;
    Rigidbody2D _body;

    public Animator animator;
    bool isBouncing = false;
    public bool gunEnabled;
    const float MAX_HP = 1f;
    static float HP;
    private float nextShootTime;
    private int bulletNum = 0;
    private GameObject gun;

    public static float totalHpReduced = 0f;
    public static float dmgBySword = 0f;
    public static float dmgByGun = 0f;
    public static Move _move;

    // footprints-related variables
    private Vector2 _prevPos;
    private Vector2 _curPos;
    [SerializeField] GameObject FootPrint;
    [SerializeField] float FootprintGap = 1f;

    // Portal-related variables
    [SerializeField] GameObject Portal;
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

    private void Awake() {
        _move = this;//static this scirpts for other scripts to deploy
    }

    void Start() {
        _body = GetComponent<Rigidbody2D>();
        swordPivot.SetActive(false);
        gunEnabled = false;
        HP = MAX_HP;
        GameManager.instance.DeathUI.SetActive(false);
        _prevPos = transform.position;
        _renderer = GetComponent<SpriteRenderer>();
        _holdingProgressRendererR = holdingProgressR.GetComponent<Renderer>();
        _holdingProgressRendererT = holdingProgressT.GetComponent<Renderer>();
    }

    void Update() {
        fieldOfView.SetOrigin(transform.position);
        animator.SetFloat("Speed", Vector3.Magnitude(_body.velocity));
        if(_renderer.enabled) {
            DropFootprint();
            FlipPlayer();

        }
        // channeling the portal/etc.
        if(Input.GetKeyDown(KeyCode.R)) {
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

        if(bulletNum > 0 && (Input.GetMouseButton(0) || Input.GetKeyDown("space")) && Time.time >= nextShootTime) {
            nextShootTime = Time.time + 0.6f;
            bulletNum--;
            Instantiate(playerBullet, this.transform.position, this.transform.rotation);
            if(bulletNum > 0) {
                GameManager.instance.BulletUI.text = bulletNum.ToString();
            } else {
                GameManager.instance.BulletUI.text = "";
            }
        }


        if(_hurtTimer > 0) {
            _hurtTimer -= Time.deltaTime;
            if(_hurtTimer < 0) {
                StartCoroutine(StopHurtAnimation());
                _hurtTimer = 0f;
            }
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

    private void OnCollisionEnter2D(Collision2D other) {
        // Debug.Log(other.collider.name);
    }

    void Run() {
        Vector2 moveSpeed = new Vector2(moveInput.x * speed, moveInput.y * speed) * Time.deltaTime;
        _body.velocity = moveSpeed;
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    void FlipPlayer() {
        if(!swordPivot.activeSelf) {
            bool hasHorizontalSpeed = Mathf.Abs(_body.velocity.x) > Mathf.Epsilon;
            if(hasHorizontalSpeed) {
                transform.localScale = new Vector2(Mathf.Sign(_body.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
        } else {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.localScale = new Vector2(
                Mathf.Sign(diff.x) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y
            );
        }

        if(transform.localScale.x <= 0 && holdingProgressT.transform.localScale.x >= 0 ||
            transform.localScale.x >= 0 && holdingProgressT.transform.localScale.x <= 0) {
            Vector3 scale = holdingProgressT.transform.localScale;
            scale.x *= -1;
            holdingProgressT.transform.localScale = scale;
            holdingProgressR.transform.localScale = scale;
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

    public bool EnableSword() {
        this.swordPivot.SetActive(true);
        return true;
    }

    public bool EnableGun(GameObject gun) {
        bulletNum += 20;
        gunEnabled = true;
        return true;
    }

    public bool SwordEnabled() {
        return this.swordPivot.activeSelf;
    }
    public bool GunEnabled() {
        return gunEnabled;
    }

    private void DropFootprint() {
        _curPos = transform.position;
        float dist = Mathf.Sqrt(Mathf.Pow(_curPos.x - _prevPos.x, 2) + Mathf.Pow(_curPos.y - _prevPos.y, 2));
        if(dist > FootprintGap) {
            float tanVal = (_curPos.y - _prevPos.y) / (_curPos.x - _prevPos.x);
            float rotationAngle = Mathf.Atan(tanVal) * (180 / Mathf.PI) + ((_curPos.x - _prevPos.x) < 0 ? 180 : 0);
            Instantiate(FootPrint, transform.position, Quaternion.Euler(new Vector3(0, 0, rotationAngle)));
            _prevPos = _curPos;
        }
    }

    public IEnumerator Die() {
        this.swordPivot.SetActive(false);
        animator.SetBool("isDie", true);
        yield return new WaitForEndOfFrame();
    }

    public bool isTeleporting() {
        return _teleporting;
    }
}
