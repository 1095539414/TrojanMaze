using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDecrease : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        gameObject.layer = 7;

    }

    // Update is called once per frame
    void Update() {

    }

    float decreaseRatio = 2f;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            Move._move.speed /= decreaseRatio;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            Move._move.speed *= decreaseRatio;
        }
    }
}
