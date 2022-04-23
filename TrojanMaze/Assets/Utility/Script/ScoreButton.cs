using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreButton : MonoBehaviour
{
    public GameObject ScorePanel;

    public void OpenPanel() {
        if(ScorePanel != null) {
            bool isActive = ScorePanel.activeSelf;
            ScorePanel.SetActive(!isActive);
            if(!isActive) {
                 PrintScore();
            }
        }
    }

    public void PrintScore() {
        GameManager.instance.ScoreTextUI.text = 
            "Zombies killed: " + ScoreScript.GetKilledZombies() + 
            "\n" +  "Time spent: " + ScoreScript.GetTimeSpent() + 
            "\n" + "Overall Score:" + ScoreScript.GetOverallScore();
    }
}
