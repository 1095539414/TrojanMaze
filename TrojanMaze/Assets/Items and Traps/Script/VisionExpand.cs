using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionExpand : BuffItem
{
    // Start is called before the first frame update
    public float time = 3f;

    protected override bool AddBuff() {
        Move.viewPotionTimer = time;
        Move._move.BoostView_Effectopen();
        name = gameObject.name;
        status.AddBuff(name,GetComponent<SpriteRenderer>().sprite, GetDuration(), this.header, this.content);
        return true;
    }

    protected override bool RemoveBuff() {
        status.RemoveBuff(name);
        return true;
    }

    protected override float GetDuration() {
        return time;
    }
}
