using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffItem : MonoBehaviour {
    private InventoryManager inventory;
    public GameObject itemButton;
    private int slotNum;
    protected bool used;

    protected GameObject buffTarget;

    protected State status;
    public string header;
    public string content;


    //[SerializeField]
    public Sprite sprite;
    private static LTDescr delay;

    private static GameObject objectShowing;

    public Image durationImage;
    public Image durationBackgroundImage;
    private float durationElapsedTime;
    private float durationTotalTime;

    public void Initialize(string name, Sprite sprite, float duration, string header = "", string content = "") {
        durationImage.sprite = sprite;
        this.name = name;
        durationElapsedTime = duration;
        durationTotalTime = duration;
        GetComponent<TooltipTrigger>().header = header;
        GetComponent<TooltipTrigger>().content = content;
    }

    // Start is called before the first frame update
    void Start() {
        GameObject canvas = GameObject.Find("Canvas");
        inventory = canvas.GetComponent<InventoryManager>();
        status = canvas.GetComponent<State>();
        gameObject.layer = 7;

    }

    // Update is called once per frame
    void Update() {
        if(durationImage != null && durationBackgroundImage != null && durationElapsedTime > 0) {
            durationElapsedTime -= Time.deltaTime;
            durationImage.fillAmount = durationElapsedTime / durationTotalTime;
            durationBackgroundImage.fillAmount = durationElapsedTime / durationTotalTime;
        }
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


    private void OnMouseEnter() {
        delay = LeanTween.delayedCall(0.5f, () => {
            objectShowing = this.gameObject;
            TooltipManager.instance.Show(content, header, this.gameObject);
        });
    }

    private void OnMouseExit() {
        if(delay != null) {
            LeanTween.cancel(delay.uniqueId);
        }
        TooltipManager.instance.Hide(gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other) {

        buffTarget = other.gameObject;
        if(buffTarget.CompareTag("Player") && buffTarget.GetComponent<Move>().isTeleporting()) {
            return;
        }

        if(other.CompareTag("Player") || other.gameObject.CompareTag("Armor")) {
            if(gameObject.CompareTag("gun") || gameObject.CompareTag("sword") || gameObject.CompareTag("ZombieBullet")) {
                if(AddBuff()) {
                    Invoke("RemoveBuff", GetDuration());
                }
                if(gameObject.CompareTag("sword")) {
                    if(Input.GetKeyDown(KeyCode.C) || Input.GetMouseButton(1)) {
                        this.gameObject.SetActive(false);
                    }
                } else {
                    this.gameObject.SetActive(false);
                }
                if(!this.CompareTag("ZombieBullet")) {
                    UnityAnalytics.sendItemCollected(this.name);
                }
            } else if(gameObject.CompareTag("Hpincrease") && Move.GetHP() + 0.2f <= 1f) {
                if(AddBuff()) {
                    Invoke("RemoveBuff", GetDuration());
                }
                this.gameObject.SetActive(false);
            } else {
                InventoryManager.instance.AddItem(this.gameObject);
            }
        } else if(other.gameObject.CompareTag("Walls")) {
            this.gameObject.SetActive(false);
        }
    }

    public void Activate() {
        if(AddBuff()) {
            UnityAnalytics.sendItemCollected(this.name);
            Invoke("RemoveBuff", GetDuration());
        }
    }

}
