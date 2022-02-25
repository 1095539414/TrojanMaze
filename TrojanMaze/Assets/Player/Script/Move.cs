using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class Move : MonoBehaviour, iDamageable {
    [SerializeField] public float speed = 10f;
    [SerializeField] private FieldOfView filedOfView;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] float fireInterval = 1f;
    [SerializeField] GameObject swordPivot;
    Vector2 moveInput;
    Rigidbody2D rigidBody;

    bool isBouncing = false;
    public bool gunEnabled;
    const float MAX_HP = 1f;
    static float HP;

    public static Move _move;

    private void Awake() {
        _move = this;//static this scirpts for other scripts to deploy
    }

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        swordPivot.SetActive(false);
        HP = MAX_HP;
    }

    void Update() {
        //FlipPlayer();
        filedOfView.SetOrigin(transform.position);
        if(gunEnabled && Input.GetKeyDown(KeyCode.Space)) {
            Instantiate(bullet, gun.position, gun.rotation);
        }

        if(HP > 1) {
            HP = 1;
        }
        // mute it because if it is kept,when hp is reduced to 0 by a trap, cannot reload the scene and send the player to the start position
        /*if (HP <= 0) {
            Destroy(gameObject);
        }*/
    }

    void FixedUpdate() {
        Run();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // Debug.Log(other.collider.name);
    }

    void Run() {
        Vector2 moveSpeed = new Vector2(moveInput.x * speed, moveInput.y * speed) * Time.deltaTime;
        rigidBody.velocity = moveSpeed;
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    void FlipPlayer() {
        bool hasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        if(hasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
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

    public bool ReduceHealth(float value) {
        if(HP > 0) {
            HP -= value;
            if(HP <= 0) {
                AnalyticsResult analyticsResult = Analytics.CustomEvent(
                    "LevelDied",
                    new Dictionary<string, object>{
                        {"Level", SceneManager.GetActiveScene().name}
                    }
                );
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
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

    public bool EnableGun() {
        gunEnabled = true;
        return true;
    }

    public bool SwordEnabled() {
        return this.swordPivot.activeSelf;
    }
    public bool GunEnabled() {
        return gunEnabled;
    }
}
