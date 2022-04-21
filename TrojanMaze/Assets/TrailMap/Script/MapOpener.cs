using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpener : MonoBehaviour
{
    public GameObject miniMap;

    void Update() {
        if(Input.GetKeyDown(KeyCode.V)) {
            OpenPanel();
        }
    }

    public void OpenPanel() {
        if(miniMap != null) {
            bool isActive = miniMap.activeSelf;
            miniMap.SetActive(!isActive);
        }
    }
}
