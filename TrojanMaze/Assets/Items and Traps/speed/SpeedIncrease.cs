using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///speed increase
/// </summary>
public class SpeedIncrease : MonoBehaviour
{
    public float time = 3f;

    float newMove = 500f;


    /// <summary>
    /// //
    /// </summary>
    public static SpeedIncrease _speedIncrease;


    void test()
    {
        Debug.Log(SpeedDecrease._speedDecrease.time);
    }
    private void Awake()
    {
        _speedIncrease = this;//static this scirpts for other scripts to deploy
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy it when player collide it
        if (other.gameObject.CompareTag("Player"))
        {

            Move._move.speed = newMove;
            Invoke("oldmove", time);   //deploy this to return to the old speed in 3s
            GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);//change to transparency to(0, 0, 0, 0) 
        }
    }

    void oldmove()
    {
        newMove = 300f;
        Move._move.speed = newMove;
        Destroy(gameObject);
    }
}
