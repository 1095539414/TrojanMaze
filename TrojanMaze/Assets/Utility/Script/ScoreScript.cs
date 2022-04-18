using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TMP;

    private static int KillNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        TMP.text = "Killed Zombies: 0"; 
    }

    // Update is called once per frame
    void Update()
    {
         TMP.text = "Killed Zombies: " + KillNum; 
    }

    public static void IncreaseKillNum() {
        KillNum++;
    }

    public static void ResetKillNum() {
        KillNum = 0;
    }
}
