using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    // Start is called before the first frame update
    private static LTDescr delay;
    public string header;
    public string content;
    private void OnMouseEnter() {
        ShowTooltip();
    }

    private void OnMouseExit() {
        HideTooltip();

    }

    public void OnPointerEnter(PointerEventData eventData) {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData) {
        HideTooltip();
    }

    private void ShowTooltip() {
        if(content.Length > 0) {
            delay = LeanTween.delayedCall(0.5f, () => {
                TooltipManager.instance.Show(content, header, this.gameObject);
            });
        }
    }

    private void HideTooltip() {
        if(delay != null) {
            LeanTween.cancel(delay.uniqueId);
        }
        TooltipManager.instance.Hide();
    }
    public void SetText(string content, string header = "") {
        this.content = content;
        this.header = header;
    }

    public void Reset() {
        this.content = "";
        this.header = "";
    }
}
