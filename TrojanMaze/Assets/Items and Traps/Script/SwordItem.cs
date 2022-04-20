using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordItem : BuffItem {
    // Start is called before the first frame update
    float healAmount = 0.3f;

    private GameObject trailMapIcon;

    void Start() {
        trailMapIcon = transform.GetChild(0).gameObject;
    }

    void Update() {
        if(State.getTowerState()) {
            trailMapIcon.GetComponent<Renderer> ().enabled = true;
        } else {
            trailMapIcon.GetComponent<Renderer> ().enabled = false;
        }
    }
    protected override bool AddBuff() {
        if(!buffTarget.GetComponent<Move>().SwordEnabled()) {
            buffTarget.GetComponent<Move>().EnableSword();
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
