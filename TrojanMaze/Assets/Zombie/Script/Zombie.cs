using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class Zombie : MonoBehaviour, iDamageable {
    private float _health;
    private Coroutine _hurtCoroutine;
    private bool _hurt = false;
    // Getters
    public float health {
        get { return _health; }
    }

    // used by its inheritance
    protected void Init(float health) {
        _health = health;
        this.gameObject.tag = "Zombie";
    }

    // Health calculation 
    // other factors that are influenced by helath could be done here
    public bool ReduceHealth(float amount, GameObject from) {
        if(_health > 0 && !_hurt) {
            _health -= amount;
            if(_health <= 0.001) {
                StartCoroutine(Die());
            } else {
                _hurt = true;
                _hurtCoroutine = StartCoroutine(Hurt());
            }
        }
        return true;
    }

    protected bool Attack(GameObject obj, float damage) {
        iDamageable damageableObj = obj.GetComponent<iDamageable>();
        if(damageableObj != null) {
            return damageableObj.ReduceHealth(damage, this.gameObject);
        }
        return false;
    }


    public virtual IEnumerator Die() {
        UnityAnalytics.sendZombieKilled(this.name);
        Destroy(this.gameObject);
        yield return null;
    }

    public virtual IEnumerator Hurt() {
        _hurt = false;
        yield return null;
    }
}
