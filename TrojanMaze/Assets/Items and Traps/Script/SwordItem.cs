using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordItem : BuffItem {
    // Start is called before the first frame update
    float healAmount = 0.3f;
    protected override bool AddBuff() {
        if(!buffTarget.GetComponent<Move>().SwordEnabled()) {
            buffTarget.GetComponent<Move>().EnableSword();
            name = gameObject.name;
            status.AddBuff(spriteR, name);
            return true;
        }
        return false;
    }

    protected override bool RemoveBuff() {
        return false;
    }

    protected override float GetDuration() {
        return 0f;
    }
}
