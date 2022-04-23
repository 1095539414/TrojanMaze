using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TMP;

    private static int KillNum = 0;
    private static float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        KillNum = 0;
        time = 0f;
        TMP.text = "Killed Zombies: 0"; 
        GameManager.instance.TimeSpent.text = "0s"; 
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        TMP.text = "Killed Zombies: " + KillNum; 
        GameManager.instance.TimeSpent.text = Mathf.Floor(time) + "s"; 
    }

    public static void IncreaseKillNum() {
        KillNum++;
    }

    public static void ResetKillNum() {
        KillNum = 0;
    }

    public static int GetKilledZombies() {
        return KillNum;
    }

    public static int GetTimeSpent() {
        return (int)time;
    }

    public static int GetOverallScore() {
        int hitScore = 100*KillNum;
        /*
            timeScore
            <2min : 500
            2min ~ 6min: (6*60 - time)* 500 / (4*60)
            >=6min: 0
        */
        int timeScore = time < 2*60 ? 500 : (time > 6*60 ? 0 : (int)((6*60 - time) * 500 / 4*60));
        int resistanceScore = (int)(100*Move.totalHpReduced + 10*Move.dmgByGun + 10*Move.dmgBySword);
        return hitScore +  timeScore + resistanceScore;
    }

}
