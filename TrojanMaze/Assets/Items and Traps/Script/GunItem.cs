using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : BuffItem {
    // Start is called before the first frame update

    private int bulletNum = 20;
    
    protected override bool AddBuff() {
        buffTarget.GetComponent<Move>().EnableGun(this.gameObject);
        return true;
    }

    protected override bool RemoveBuff() {
        return false;
    }

    protected override float GetDuration() {
        return 0f;
    }

    public int getBulletNum()
    {
        return bulletNum;
    }

    public void shoot()
    {
        bulletNum--;
    }
}
