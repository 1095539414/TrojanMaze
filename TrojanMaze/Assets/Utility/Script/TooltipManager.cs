using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour {
    public static TooltipManager instance;

    public Tooltip tooltip;
    private GameObject ObjShown;
    private void Awake() {
        instance = this;
    }

    private void Update() {
        if(ObjShown != null && !ObjShown.activeSelf) {
            Hide();
        }
    }
    public void Show(string content, string header = "", GameObject obj = null) {
        ObjShown = obj;
        instance.tooltip.SetText(content, header);
        instance.tooltip.gameObject.SetActive(true);
    }

    public void Hide() {
        ObjShown = null;
        instance.tooltip.gameObject.SetActive(false);
    }
}
