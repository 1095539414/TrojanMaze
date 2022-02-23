using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BuffItem : MonoBehaviour {
    protected GameObject buffTarget;

    protected State state;

    public string name;

    [SerializeField]
    protected SpriteRenderer spriteR;

    // [SerializeField]
    // private Image durationImg;
    
    [SerializeField]
    private Image icon;

    public void Initialize(SpriteRenderer spriteR,string name){
        this.icon.sprite = spriteR.sprite;
        this.icon.color = spriteR.color;
        this.name = name;
        //this.durationImg.fillAmount = 0;
    }

    // Start is called before the first frame update
    void Start() {
        GameObject canvas = GameObject.Find("Canvas");
        state = canvas.GetComponent<State>();
    }

    // Update is called once per frame
    void Update() {
        //durationImg.fillAmount = 
    }
    
    // how do I apply the buff
    protected virtual bool AddBuff(){
        return false;
    }
    // how do I take off the buff
    protected virtual bool RemoveBuff(){
        return false;
    }
    // How long this buff lasts
    protected virtual float GetDuration(){
        return 0;
    }
    protected void OnTriggerEnter2D(Collider2D other) {
        buffTarget = other.gameObject;
        if(other.gameObject.CompareTag("Player")) {
            if(AddBuff()) {
                Invoke("RemoveBuff", GetDuration());
            }
            this.gameObject.SetActive(false);
            SendAnalytic();
        } else if(other.gameObject.CompareTag("Walls")) {
            this.gameObject.SetActive(false);
        }
    }

    private void SendAnalytic() {
        // Analytics counting the amount of times each item is used in each level
        AnalyticsResult analyticsResult = Analytics.CustomEvent(
            "ItemCollected",
            new Dictionary<string, object>{
                {SceneManager.GetActiveScene().name, this.gameObject.name}
            }
        );
    }
}
