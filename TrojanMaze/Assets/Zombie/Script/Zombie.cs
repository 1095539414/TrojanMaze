using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour, iDamageable {
    private float _health;

    // Getters
    public float health {
        get { return _health; }
    }

    // used by its inheritance
    protected void Init(int health) {
        _health = health;
        this.gameObject.tag = "Zombie";
    }

    // Health calculation 
    // other factors that are influenced by helath could be done here
    public bool ReduceHealth(float amount) {
        _health -= amount;
        Debug.Log(_health);
        if(_health <= 0) {
            StartCoroutine(Die());
        }
        return true;
    }

    protected bool Attack(GameObject obj, float damage) {
        iDamageable damageableObj = obj.GetComponent<iDamageable>();
        if(damageableObj != null) {
            return damageableObj.ReduceHealth(damage);
        }
        return false;
    }

    private IEnumerator Die() {
        // Death Animation here
        Destroy(this.gameObject);
        // wait for a bit before erasing the zombie
        yield return new WaitForSeconds(1.5f);
        
    }
}
