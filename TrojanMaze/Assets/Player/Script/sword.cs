using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{

    float swordDamage = 0.5f;
    [SerializeField] GameObject swordPivot;

    public Animator animator;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zombie") && !other.isTrigger)
        {
            iDamageable damageableObj = other.gameObject.GetComponent<iDamageable>();
            if (damageableObj != null)
            {
                if (damageableObj.ReduceHealth(swordDamage, this.gameObject))
                {
                    Move.dmgBySword += swordDamage;
                }
            }
        }
    }
}
