using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordItem : MonoBehaviour {
    // Start is called before the first frame update
    float healAmount = 0.3f;

    private GameObject trailMapIcon;

    void Start() {
        gameObject.layer = 7;
        trailMapIcon = transform.GetChild(0).gameObject;
    }

    void Update() {
        if(State.getTowerState()) {
            trailMapIcon.GetComponent<Renderer>().enabled = true;
        } else {
            trailMapIcon.GetComponent<Renderer>().enabled = false;
        }
    }


    protected void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            Move._move.WeaponTouched(this.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            Move._move.WeaponUntouched(this.gameObject);
        }
    }
}
