using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
///speed decrease
/// </summary>
public class SpeedDecrease : BuffItem {
    public float time = 3f;

    float decreaseRatio = 2f;
    protected override bool AddBuff() {
        Move._move.speed /= decreaseRatio;
        name = gameObject.name;
        status.AddBuff(name,icon);
        return true;
    }

    protected override bool RemoveBuff() {
        Move._move.speed *= decreaseRatio;
        status.RemoveBuff(name);
        return true;
    }

    protected override float GetDuration() {
        return time;
    }
}
