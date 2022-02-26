using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : BuffItem {
    // Start is called before the first frame update
    float healAmount = 0.3f;
    protected override bool AddBuff() {
            buffTarget.GetComponent<Move>().EnableGun();
            if(buffTarget.GetComponent<Move>().bulletNum == -1){
                name = gameObject.name;
                state.AddBuff(spriteR, name);
                buffTarget.GetComponent<Move>().bulletNum ++;
            }
            buffTarget.GetComponent<Move>().bulletNum += 20;
            return true;
        return false;
    }

    protected override bool RemoveBuff() {
        return false;
    }

    protected override float GetDuration() {
        return 0f;
    }
}
