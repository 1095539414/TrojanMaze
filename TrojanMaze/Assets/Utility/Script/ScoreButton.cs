using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreButton : MonoBehaviour {
    public GameObject ScorePanel;
    public GameObject ScoreButtonObj;
    public GameObject ScoreButtonPressedObj;
    private void Start() {
        ScorePanel.SetActive(false);
        ScoreButtonObj.SetActive(true);
        ScoreButtonPressedObj.SetActive(false);
    }
    public void OpenPanel() {
        if(ScorePanel != null) {
            bool isActive = ScorePanel.activeSelf;
            ScorePanel.SetActive(!isActive);
            if(!isActive) {
                PrintScore();
            }
            ScoreButtonObj.SetActive(isActive);
            ScoreButtonPressedObj.SetActive(!isActive);
        }
    }

    public void PrintScore() {
        GameManager.instance.ScoreTextUI.text =
            "Zombies Killed: " + ScoreScript.GetKilledZombies() +
            "\n" + "Time Spent: " + ScoreScript.GetTimeSpent() +
            "\n" + "Resistance Ability: " + (int)(10 * Move.totalHpReduced) +
            "\n" + "Gun Damage: " + (int)Move.dmgByGun +
            "\n" + "Sword Damage: " + (int)Move.dmgBySword +
            "\n" + "Overall Score: " + ScoreScript.GetOverallScore();
    }
}
