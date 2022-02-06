using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    [SerializeField] GameObject ThingToFollow;

    void LateUpdate() {
        if (ThingToFollow)
        {
            transform.position = ThingToFollow.transform.position + new Vector3(0, 0, -10);
        }
    }
}
