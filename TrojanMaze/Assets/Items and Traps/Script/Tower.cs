using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Camera myCamera;
    float initialCamera;
    float boostCamera = 10f;
    float cameraZoom;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
        initialCamera = myCamera.orthographicSize;
        cameraZoom = initialCamera;
    }

    // Update is called once per frame
    void Update()
    {
        zoom();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            cameraZoom = boostCamera;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") {
            cameraZoom = initialCamera;
        }
    }

    private void zoom(){
        float cameraZoomDifference = cameraZoom - myCamera.orthographicSize;
        float cameraZoomSpeed= 1f;

        myCamera.orthographicSize += cameraZoomDifference * cameraZoomSpeed * Time.deltaTime;

        if(cameraZoomDifference > 0 ){
            if(myCamera.orthographicSize > cameraZoom){
                myCamera.orthographicSize = cameraZoom;
            }
        }else{
            if(myCamera.orthographicSize < cameraZoom){
                myCamera.orthographicSize = cameraZoom;
            }
        }
    }
}
