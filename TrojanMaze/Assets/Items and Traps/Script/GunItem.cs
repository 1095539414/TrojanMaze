using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour {
    // Start is called before the first frame update
    private int bulletNum = 20;
    private GameObject trailMapIcon;

    public int getBulletNum() {
        return bulletNum;
    }

    public int UpdateBulletNum(int newNum) {
        int curNum = bulletNum;
        bulletNum = newNum;
        return curNum;
    }
    public void setBulletNum(int num) {
        bulletNum = num;
    }

    void Start() {
        gameObject.layer = 7;
        trailMapIcon = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
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
