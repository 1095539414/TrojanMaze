using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionExpand : BuffItem
{
    // Start is called before the first frame update
    public float time = 3f;


    protected override bool AddBuff() {
        FieldOfView.BoostViewDistance();
        name = gameObject.name;
        status.AddBuff(spriteR,name);
        return true;
    }

    protected override bool RemoveBuff() {
        FieldOfView.ResetViewDistance();
        status.RemoveBuff(name);
        return true;
    }

    protected override float GetDuration() {
        return time;
    }
}
