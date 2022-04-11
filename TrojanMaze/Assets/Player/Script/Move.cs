using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Move : MonoBehaviour, iDamageable {
    [SerializeField] public float speed = 10f;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] float fireInterval = 1f;
    [SerializeField] GameObject swordPivot;
    [SerializeField] GameObject playerBullet;
    [SerializeField] GameObject holdingProgress;
    Vector2 moveInput;
    Rigidbody2D _body;

    public Animator animator;

    [SerializeField] private TextMeshProUGUI BulletText;
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
    private float _holdTimer = 0f;
    private bool _isHolding = false;

    private SpriteRenderer _renderer;
    private float _hurtTimer = 0f;
    public static Dictionary<string, float> damagedFrom = new Dictionary<string, float>();


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
    }

    void Update() {

        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) {
            animator.SetBool("isWalk", true);
        } else {
            animator.SetBool("isWalk", false);
        }

        // make sure the loading progress always display corectly
        if(transform.localScale.x <= 0 && holdingProgress.transform.localScale.x >= 0 ||
            transform.localScale.x >= 0 && holdingProgress.transform.localScale.x <= 0) {
            Vector3 scale = holdingProgress.transform.localScale;
            scale.x *= -1;
            holdingProgress.transform.localScale = scale;
        }

        // channeling the portal/etc.
        if(Input.GetKeyDown(KeyCode.R)) {
            if(_portal == null) {
                _portal = Instantiate(Portal, transform.position, transform.rotation);
            } else {
                _isHolding = true;
            }
        }

        // reset the channeling
        if(Input.GetKeyUp(KeyCode.R)) {
            _isHolding = false;
            _holdTimer = 0f;
        }

        // update channeling status
        if(_isHolding) {
            holdingProgress.SetActive(true);
            holdingProgress.GetComponent<Renderer>().sharedMaterial.SetFloat(
                "_Arc2", 360f - _holdTimer / _holdTimerTarget * 360f
            );

            _holdTimer += Time.deltaTime;
            _body.velocity = Vector3.zero;
            if(_holdTimer >= _holdTimerTarget) {
                _holdTimer = -100f;
                StartCoroutine(TeleportBack());
            }
            return;
        } else {
            holdingProgress.SetActive(false);
            holdingProgress.GetComponent<Renderer>().sharedMaterial.SetFloat("_Arc2", 360f);
        }

        if(!swordPivot.activeSelf) {
            FlipPlayer();
        } else {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.localScale = new Vector2(
                Mathf.Sign(diff.x) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y
            );
        }

        fieldOfView.SetOrigin(transform.position);
        if(bulletNum > 0 && (Input.GetMouseButton(0) || Input.GetKeyDown("space")) && Time.time >= nextShootTime) {
            nextShootTime = Time.time + 0.6f;
            bulletNum--;
            Instantiate(playerBullet, this.transform.position, this.transform.rotation);
        }

        if(bulletNum > 0) {
            BulletText.text = bulletNum.ToString();
        } else {
            BulletText.text = "";
        }

        if(_hurtTimer > 0) {
            _hurtTimer -= Time.deltaTime;
            if(_hurtTimer < 0) {
                StartCoroutine(StopHurtAnimation());
                _hurtTimer = 0f;
            }
        }
        GameManager.instance.PortalUI.SetActive(_portal == null);
        DropFootprint();
    }

    IEnumerator TeleportBack() {
        float elapsedTime = 0f;
        float waitTime = 0.08f;
        Vector3 originalScale = transform.localScale;
        Vector3 finalScale = new Vector3(0, originalScale.y * 1.5f, originalScale.z);
        while(elapsedTime <= waitTime) {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, finalScale, elapsedTime / waitTime);
            yield return null;
        }
        fieldOfView.enabled = false;

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
        elapsedTime = 0f;
        waitTime = 0.08f;
        while(elapsedTime <= waitTime) {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(finalScale, originalScale, elapsedTime / waitTime);
            yield return null;
        }
        Destroy(_portal);
        _portal = null;
        _isHolding = false;
        _holdTimer = 0f;
    }

    void FixedUpdate() {
        if(!_isHolding) {
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
        bool hasHorizontalSpeed = Mathf.Abs(_body.velocity.x) > Mathf.Epsilon;
        if(hasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(_body.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
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
        if(HP > 0) {
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
}
