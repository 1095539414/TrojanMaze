using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///speed increase
/// </summary>
public class SpeedIncrease : BuffItem {
    public float time = 3f;

    float increaseRatio = 1.5f;

    protected override bool AddBuff() {
        Move._move.SpeedIncrease_Effectopen();
        name = gameObject.name;
        status.AddBuff(name, GetComponent<SpriteRenderer>().sprite, GetDuration(), this.header, this.content);
        Move.speedPotionTimer = time;
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
