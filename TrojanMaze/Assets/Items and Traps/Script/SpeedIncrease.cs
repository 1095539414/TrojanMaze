using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///speed increase
/// </summary>
public class SpeedIncrease : BuffItem {
    public float time = 3f;

    float increaseRatio = 2f;

    protected override bool AddBuff() {
        Move._move.speed *= increaseRatio;
        return true;
    }

    protected override bool RemoveBuff() {
        Move._move.speed /= increaseRatio;
        return true;
    }

    protected override float GetDuration() {
        return time;
    }
}
