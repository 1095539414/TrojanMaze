using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusZone : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float ValidityPeriod = 1f;
    private float _time = 0f;

    [SerializeField] float virusDamage = 0.02f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if(_time > ValidityPeriod) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            iDamageable damageableObj = other.gameObject.GetComponent<iDamageable>();
            if(damageableObj != null) {
                damageableObj.ReduceHealth(virusDamage);
            }
        }
    }
}
