using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour {
    public static GameEvents instance;
    public event Action onTowerEnterTrigger;
    public event Action onTowerExitTrigger;

    private void Awake() {
        instance = this;
    }

    public void TowerEnterTrigger() {
        if(onTowerEnterTrigger != null) {
            onTowerEnterTrigger();
        }
    }
    public void TowerExitTrigger() {
        if(onTowerExitTrigger != null) {
            onTowerExitTrigger();
        }
    }
}
