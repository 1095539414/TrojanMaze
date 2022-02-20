using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
///speed decrease
/// </summary>
public class SpeedDecrease : MonoBehaviour {
    public float time = 3f;
    float newMove = 100f;

    /// <summary>
    /// //make this static for other scripts to deploy
    /// </summary>
    public static SpeedDecrease _speedDecrease;

    private void Awake() {
        _speedDecrease = this;//static this scirpts for other scripts to deploy
    }




    private void Update() {
        Debug.Log(Move._move.speed);
    }
    void OnTriggerEnter2D(Collider2D other) {
        //Destroy it when player collide it
        if(other.gameObject.CompareTag("Player")) {

            Move._move.speed = newMove;
            Invoke("oldmove", time);// // deploy this to return to the old speed in 3s

            GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);////change to transparency to(0, 0, 0, 0) 



        }
    }

    void oldmove() {
        newMove = 300f;
        Move._move.speed = newMove;
        Destroy(gameObject);//destroy the item after 3s
    }
}
