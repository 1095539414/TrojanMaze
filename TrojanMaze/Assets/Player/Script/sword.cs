using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{

    float swordDamage = 0.2f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zombie") || other.CompareTag("Walls"))
        {
            iDamageable damageableObj = other.gameObject.GetComponent<iDamageable>();
            if (damageableObj != null)
            {
                damageableObj.ReduceHealth(swordDamage);
            }
        }
    }
}
