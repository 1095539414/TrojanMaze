using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusZone : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] float ValidityPeriod = 1f;
    private float _time = 0f;

    [SerializeField] float virusDamage = 0.002f;

    float _damageInterval = 0.1f;
    float _damageTime = 0f;
    GameObject _damageTarget;
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(_damageTarget) {
            _damageTime += Time.deltaTime;
            if(_damageTime >= _damageInterval) {
                iDamageable damageableObj = _damageTarget.gameObject.GetComponent<iDamageable>();
                if(damageableObj != null) {
                    damageableObj.ReduceHealth(virusDamage, this.gameObject);
                }
                _damageTime = 0f;
            }
        }
        _time += Time.deltaTime;
        if(_time > ValidityPeriod) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            _damageTarget = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            _damageTarget = null;
        }
    }
}
