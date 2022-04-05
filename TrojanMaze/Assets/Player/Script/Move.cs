using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Move : MonoBehaviour, iDamageable {
    [SerializeField] public float speed = 10f;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] float fireInterval = 1f;
    [SerializeField] GameObject swordPivot;
    [SerializeField] GameObject playerBullet;
    [SerializeField] GameObject menuPanel;
    Vector2 moveInput;
    Rigidbody2D _body;


    [SerializeField]
    private TextMeshProUGUI BulletText;
    [SerializeField]
    private TextMeshProUGUI ProtalText;

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
    [SerializeField] int NumOfProtals = 3;

    public static Dictionary<string, float> damagedFrom = new Dictionary<string, float>();

    private void Awake() {
        _move = this;//static this scirpts for other scripts to deploy
    }

    void Start() {
        _body = GetComponent<Rigidbody2D>();
        swordPivot.SetActive(false);
        gunEnabled = false;
        HP = MAX_HP;
        menuPanel.SetActive(false);
        _prevPos = transform.position;
    }

    void Update() {
        FlipPlayer();
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

        ProtalText.text = NumOfProtals.ToString();

        DropFootprint();
        MarkLocation();
    }

    void FixedUpdate() {
        Run();
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

    public bool ReduceHealth(float value, GameObject from) {
        if(HP > 0) {
            HP -= value;
            totalHpReduced += value;
            if(!damagedFrom.ContainsKey(from.name)) {
                damagedFrom.Add(from.name, 0f);
            }
            damagedFrom[from.name] += value;

            if(HP <= 0) {
                UnityAnalytics.sendLevelDied();
                Time.timeScale = 0f;
                menuPanel.SetActive(true);
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
        if (dist > FootprintGap) {
            float tanVal = (_curPos.y - _prevPos.y) / (_curPos.x - _prevPos.x);
            float rotationAngle = Mathf.Atan(tanVal)*(180/Mathf.PI) + ((_curPos.x - _prevPos.x) < 0 ? 180: 0);
            Instantiate(FootPrint, transform.position, Quaternion.Euler(new Vector3(0, 0, rotationAngle)));
            _prevPos = _curPos;
        }
    }

    private void MarkLocation() {
        if(Input.GetKeyDown(KeyCode.M) &&  NumOfProtals > 0) {
            // Debug.Log("Press the M");
            NumOfProtals--;
            Instantiate(Portal, transform.position, transform.rotation);
        }
    }
}
