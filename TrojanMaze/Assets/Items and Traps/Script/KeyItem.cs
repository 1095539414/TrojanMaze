using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour {
    private GameObject buffTarget;
    public GameObject trailPanel;

    private void Start() {
        gameObject.layer = 7;

    }
    protected void OnTriggerEnter2D(Collider2D other) {

        buffTarget = other.gameObject;
        if(buffTarget.CompareTag("Player") && buffTarget.GetComponent<Move>().isTeleporting()) {
            return;
        }

        if(other.CompareTag("Player")) {
            if(trailPanel != null) {
                trailPanel.SetActive(true);
            }
            State.setKeyState(true);
            Debug.Log("Hello");
            this.gameObject.SetActive(false);
        }
    }
}
