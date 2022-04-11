using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///speed increase
/// </summary>
public class SpeedIncrease : BuffItem {
    public float time = 3f;

    float increaseRatio = 1.5f;

    public Animator animator;

    protected override bool AddBuff() {
        animator.SetBool("isRun", true);
        Move._move.speed *= increaseRatio;
        name = gameObject.name;
        status.AddBuff(spriteR, name);
        return true;
    }

    protected override bool RemoveBuff() {
        animator.SetBool("isRun", false);
        Move._move.speed /= increaseRatio;
        status.RemoveBuff(name);
        return true;
    }

    protected override float GetDuration() {
        return time;
    }
}
