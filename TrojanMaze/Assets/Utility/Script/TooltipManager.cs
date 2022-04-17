using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour {
    public static TooltipManager instance;

    public Tooltip tooltip;
    public void Awake() {
        instance = this;
    }

    public static void Show(string content, string header = "") {
        instance.tooltip.SetText(content, header);
        instance.tooltip.gameObject.SetActive(true);
    }

    public static void Hide() {
        instance.tooltip.gameObject.SetActive(false);
    }
}
