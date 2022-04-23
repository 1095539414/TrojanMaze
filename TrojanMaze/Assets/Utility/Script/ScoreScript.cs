using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TMP;

    private static int KillNum = 0;
    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        KillNum = 0;
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
}
