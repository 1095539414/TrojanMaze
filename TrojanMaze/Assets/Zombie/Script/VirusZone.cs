using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusZone : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float ValidityPeriod = 1f;
    private float _time = 0f;

    [SerializeField] float virusDamage = 5f;
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

    void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.CompareTag("Player")) {
            iDamageable damageableObj = other.collider.gameObject.GetComponent<iDamageable>();
            if(damageableObj != null) {
                Debug.Log("HHHHHHH");
                damageableObj.ReduceHealth(virusDamage);
            }
        }
    }
}
