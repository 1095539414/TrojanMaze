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
    public static bool PlayerOnTower;
    // Start is called before the first frame update
    void Start() {
        myCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        if(PlayerOnTower) {
            gameObject.layer = 0;
        } else {
            gameObject.layer = 7;
        }
        zoom();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            activate = 1.0f;
            if(trailPanel != null) {
                State.SetTowerState(true);
                trailPanel.SetActive(true);
            }
            PlayerOnTower = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") {
            activate = -1.0f;
            if(trailPanel != null) {
                State.SetTowerState(false);
                trailPanel.SetActive(false);
            }
            PlayerOnTower = false;

        }
    }

    private void zoom() {
        float cameraZoomSpeed = 4f;

        myCamera.orthographicSize += activate * cameraZoomSpeed * Time.deltaTime;

        if(myCamera.orthographicSize > boostCamera) {
            myCamera.orthographicSize = boostCamera;
            activate = 0f;
        }
        if(myCamera.orthographicSize < initialCamera) {
            myCamera.orthographicSize = initialCamera;
            activate = 0f;
        }
    }
}
