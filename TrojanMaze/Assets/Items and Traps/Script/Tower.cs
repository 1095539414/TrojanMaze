using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    private Camera myCamera;
    float initialCamera;
    float boostCamera = 8f;
    float cameraZoom;
    float cameraZoomSpeed = 0.1f;
    // Start is called before the first frame update
    void Start() {
        myCamera = Camera.main;
        initialCamera = myCamera.orthographicSize;
        cameraZoom = initialCamera;
    }

    // Update is called once per frame
    void Update() {
        zoom();
        Debug.Log(myCamera.orthographicSize);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {  
            cameraZoom = boostCamera;
            StartCoroutine(zoom());
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") {
            cameraZoom = initialCamera;
            StartCoroutine(zoom());
        }
    }

    IEnumerator zoom(){
        float cameraZoomDifference = Mathf.Abs(cameraZoom - myCamera.orthographicSize);
        while(myCamera.orthographicSize != cameraZoom) {
            if (myCamera.orthographicSize < cameraZoom) {
                myCamera.orthographicSize +=  cameraZoomSpeed * Time.deltaTime;
            } else {
                myCamera.orthographicSize -=  cameraZoomSpeed * Time.deltaTime; 
            }
            yield return new WaitForSeconds (0.0f); 
        }
    }
}
