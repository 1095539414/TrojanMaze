using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour {
    // Start is called before the first frame update
    private int bulletNum = 20;

    public int getBulletNum() {
        return bulletNum;
    }

    public void setBulletNum(int num) {
        bulletNum = num;
    }

    void Start() {
        gameObject.layer = 7;
    }

    // Update is called once per frame
    void Update() {

    }
    protected void OnTriggerEnter2D(Collider2D other) {

    }
}
