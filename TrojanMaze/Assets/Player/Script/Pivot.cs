using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour {

    public GameObject myPlayer;
    Rigidbody2D rigidBody;

    private void FixedUpdate() {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
     
        diff.Normalize();

        float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

       // if(rotationZ < -90 || rotationZ > 90) {
        //    transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
       // }
    }

}
