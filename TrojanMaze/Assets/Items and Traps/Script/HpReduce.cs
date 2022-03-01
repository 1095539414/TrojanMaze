using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpReduce : MonoBehaviour {
    private float _damage = 0.25f;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            iDamageable damageableObj = other.gameObject.GetComponent<iDamageable>();
            if(damageableObj != null) {
                damageableObj.ReduceHealth(_damage);
            }
        }
    }
}

