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
            "Zombies killed: " + ScoreScript.GetKilledZombies() +
            "\n" + "Time spent: " + ScoreScript.GetTimeSpent() +
            "\n" + "Resistance ability: " + 10 * Move.totalHpReduced +
            "\n" + "Gun Damage: " + Move.dmgByGun +
            "\n" + "Sword Damage: " + Move.dmgBySword +
            "\n" + "Overall Score: " + ScoreScript.GetOverallScore();
    }
}
