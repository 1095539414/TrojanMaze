using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    private Camera myCamera;
    float initialCamera = 4f;
    float boostCamera = 8f;
    float cameraZoomSpeed = 4f;
    float activate = 0f;
    // Start is called before the first frame update
    void Start() {
        myCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        zoom();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            activate = 1.0f;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") {
            activate = -1.0f;
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
