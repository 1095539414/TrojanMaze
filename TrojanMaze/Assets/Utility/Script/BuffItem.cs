using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuffItem : MonoBehaviour {
    protected GameObject buffTarget;

    protected State status;
    protected SpriteRenderer spriteR;

    // [SerializeField]
    // private Image durationImg;

    //[SerializeField]
    public Image icon;

    public void Initialize(SpriteRenderer spriteR, string name) {
        this.icon.sprite = spriteR.sprite;
        this.icon.color = spriteR.color;
        this.name = name;
    }

    // Start is called before the first frame update
    void Start() {
        GameObject canvas = GameObject.Find("Canvas");
        status = canvas.GetComponent<State>();
        spriteR = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        //durationImg.fillAmount = 
    }

    // how do I apply the buff
    protected virtual bool AddBuff() {
        return false;
    }
    // how do I take off the buff
    protected virtual bool RemoveBuff() {
        return false;
    }
    // How long this buff lasts
    protected virtual float GetDuration() {
        return 0;
    }
    protected void OnTriggerEnter2D(Collider2D other) {
        buffTarget = other.gameObject;
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Armor")){
            if(AddBuff()) {
                Invoke("RemoveBuff", GetDuration());
            }
            this.gameObject.SetActive(false);
            if(!this.CompareTag("ZombieBullet")) {
                UnityAnalytics.sendItemCollected(this.name);
            }
        } else if(other.gameObject.CompareTag("Walls")) {
            this.gameObject.SetActive(false);
        }
    }

}
