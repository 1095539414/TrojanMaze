using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;


public abstract class BuffItem : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    
    // how do I apply the buff
    protected abstract bool AddBuff();
    // how do I take off the buff
    protected abstract bool RemoveBuff();
    // How long this buff lasts
    protected abstract float GetDuration();
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            AddBuff();
            this.gameObject.SetActive(false);
            Invoke("RemoveBuff", GetDuration());

            SendAnalytic();
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
