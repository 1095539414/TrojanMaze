using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalKit : BuffItem {
    // Start is called before the first frame update
    float healAmount = 0.3f;
    private GameObject trailMapIcon;
    void Start() {
        gameObject.layer = 7;
        trailMapIcon = transform.GetChild(0).gameObject;
    }

    void Update() {
        if(trailMapIcon != null) {
            if(State.getTowerState()) {
                trailMapIcon.GetComponent<Renderer>().enabled = true;
            } else {
                trailMapIcon.GetComponent<Renderer>().enabled = false;
            }
        }
    }
    protected override bool AddBuff() {
        Move.IncreaseHP(healAmount);
        Move._move.MedicalKit_Effectopen();
        return true;
    }

    protected override bool RemoveBuff() {
        return false;
    }

    protected override float GetDuration() {
        return 0f;
    }
}
