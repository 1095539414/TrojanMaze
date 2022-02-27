using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : BuffItem
{
    public float time = 10f;
    public GameObject player;

    protected override bool AddBuff()//start the armor function
    {
        if (player.tag == "Player")
        {
            player.tag = "Armor";//change the player tag to armor so that the zombies and traps cannot find the player

        }
        status.AddBuff(spriteR, name);
        return true;
    }
    protected override bool RemoveBuff() //change the tag back. Zombies and traps could reduce HP now.
    {
        player.tag = "Player";
        status.RemoveBuff(name);
        return true;
    }
    protected override float GetDuration()
    {
        return time;
    }
}