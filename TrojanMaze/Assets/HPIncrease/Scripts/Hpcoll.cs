using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hpcoll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy it when player collide it
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Move.HP += 0.2f;  //HP Increase 
        }
    }
}
