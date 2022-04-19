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
        delay = LeanTween.delayedCall(0.5f, () => {
            TooltipManager.Show(content, header);
        });
    }

    private void OnMouseExit() {
        if(delay != null) {
            LeanTween.cancel(delay.uniqueId);
        }
        TooltipManager.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(content.Length > 0) {
            TooltipManager.Show(content, header);

        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        TooltipManager.Hide();
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
