using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuffItem : MonoBehaviour {
    private Inventory inventory;
    public GameObject itemButton;
    private int slotNum;
    protected bool used;

    protected GameObject buffTarget;

    protected State status;
    public string header;
    public string content;

    // [SerializeField]
    // private Image durationImg;

    //[SerializeField]
    public Image icon;
    private static LTDescr delay;

    public void Initialize(string name, Image iconI) {
        this.icon.sprite = iconI.sprite;
        this.name = name;
    }

    // Start is called before the first frame update
    void Start() {
        GameObject canvas = GameObject.Find("Canvas");
        inventory = canvas.GetComponent<Inventory>();
        status = canvas.GetComponent<State>();
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


    private void OnMouseEnter() {
        delay = LeanTween.delayedCall(0.5f, () => {
            TooltipManager.Show(content, header);
        });
    }

    private void OnMouseExit() {
        if(delay != null) {
            LeanTween.cancel(delay.uniqueId);
        }
        TooltipManager.Hide();
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        buffTarget = other.gameObject;
        if(buffTarget.CompareTag("Player") && buffTarget.GetComponent<Move>().isTeleporting()) {
            return;
        }

        if(other.CompareTag("Player")  || other.gameObject.CompareTag("Armor")){
            if(gameObject.CompareTag("gun")||gameObject.CompareTag("sword")||gameObject.CompareTag("ZombieBullet")){
                Debug.Log("weapon");
                if(AddBuff()) {
                    Invoke("RemoveBuff", GetDuration());
                }
                this.gameObject.SetActive(false);
                if(!this.CompareTag("ZombieBullet")) {
                    UnityAnalytics.sendItemCollected(this.name);
                }
                return;
            }
            for(int i = 0; i < inventory.slots.Length; i++){
                if(inventory.isFull[i] == false){
                    slotNum = i;
                    inventory.isFull[i] = true;
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
            UnityAnalytics.sendItemCollected(this.name);
        }else if(other.gameObject.CompareTag("Walls")){
            this.gameObject.SetActive(false);
        }
    }

    public void UseItem(){
        if(used) return;
        used = true;
        icon = gameObject.GetComponent<Image>();
        gameObject.GetComponent<Image>().enabled = !gameObject.GetComponent<Image>().enabled;
        if(AddBuff()) {
            for(int i = 0; i < inventory.slots.Length; i++){
                if(inventory.slots[i].transform.position == gameObject.transform.position){
                    inventory.isFull[i] = false;
                    break;
                }
            }
            
            Invoke("RemoveBuff", GetDuration());
        }
    }

}
