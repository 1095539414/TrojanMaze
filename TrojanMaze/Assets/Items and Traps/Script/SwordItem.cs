using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordItem : BuffItem {
    // Start is called before the first frame update
    float healAmount = 0.3f;
    protected override bool AddBuff() {
        buffTarget.GetComponent<Move>().enableSword();
        name = gameObject.name;
        state.AddBuff(spriteR,name);
        return true;
    }

    protected override bool RemoveBuff() {
        return false;
    }

    protected override float GetDuration() {
        return 0f;
    }
}
