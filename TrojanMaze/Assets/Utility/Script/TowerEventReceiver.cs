using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEventReceiver : MonoBehaviour {
    private void Start() {
        GameEvents.instance.onTowerEnterTrigger += OnTowerEnterTrigger;
        GameEvents.instance.onTowerExitTrigger += OnTowerExitTrigger;
    }

    // Start is called before the first frame update
    private void OnTowerEnterTrigger() {
        gameObject.layer = 0;
    }
    private void OnTowerExitTrigger() {
        gameObject.layer = 7;
    }

    private void OnDestroy() {
        GameEvents.instance.onTowerEnterTrigger -= OnTowerEnterTrigger;
        GameEvents.instance.onTowerExitTrigger -= OnTowerExitTrigger;
    }
}
