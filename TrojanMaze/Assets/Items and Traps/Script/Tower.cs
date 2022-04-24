using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    private Camera myCamera;
    public GameObject trailPanel;
    float initialCamera = 4f;
    float boostCamera = 8f;
    float cameraZoomSpeed = 4f;
    float activate = 0f;

    // Start is called before the first frame update
    void Start() {
        myCamera = GameManager.instance.MainCamera;
        gameObject.layer = 7;
    }

    // Update is called once per frame
    void Update() {
        zoom();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            activate = 1.0f;
            if(trailPanel != null) {
                State.SetTowerState(true);
                trailPanel.SetActive(true);
            }
            GameEvents.instance.TowerEnterTrigger();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") {
            activate = -1.0f;
            if(trailPanel != null) {
                State.SetTowerState(false);
                trailPanel.SetActive(false);
            }
            GameEvents.instance.TowerExitTrigger();

        }
    }


    private void zoom() {
        myCamera.orthographicSize += activate * cameraZoomSpeed * Time.deltaTime;
        if(myCamera.orthographicSize > boostCamera) {
            myCamera.orthographicSize = boostCamera;
            activate = 0f;
        }
        if(myCamera.orthographicSize < initialCamera - 0.01f) {
            myCamera.orthographicSize = initialCamera;
            activate = 0f;
        }
    }

}
