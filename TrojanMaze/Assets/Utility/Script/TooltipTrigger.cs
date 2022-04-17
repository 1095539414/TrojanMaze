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

    private void OnMouseDown() {
        LeanTween.cancel(delay.uniqueId);
    }
    public void OnPointerEnter(PointerEventData eventData) {
        delay = LeanTween.delayedCall(0.5f, () => {
            TooltipManager.Show(content, header);
        });
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(delay != null) {
            LeanTween.cancel(delay.uniqueId);
        }
        TooltipManager.Hide();
    }
}
