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
        Move._move.speed *= increaseRatio;
        Move._move.SpeedIncrease_Effectopen();
        name = gameObject.name;
        status.AddBuff(name, GetComponent<SpriteRenderer>().sprite);
        return true;
    }

    protected override bool RemoveBuff() {
        Move._move.speed /= increaseRatio;
        status.RemoveBuff(name);
        return true;
    }

    protected override float GetDuration() {
        return time;
    }
}
