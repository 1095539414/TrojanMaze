using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalKit : BuffItem {
    // Start is called before the first frame update
    float healAmount = 0.3f;
    protected override bool AddBuff() {
        Move.IncreaseHP(healAmount);
        return true;
    }

    protected override bool RemoveBuff() {
        return false;
    }

    protected override float GetDuration() {
        return 0f;
    }
}
