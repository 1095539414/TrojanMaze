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
            if(isActive) {
                StartCoroutine(AnimatePanelClose());
            } else {
                StartCoroutine(AnimatePanelOpen());
            }
            ScoreButtonObj.SetActive(isActive);
            ScoreButtonPressedObj.SetActive(!isActive);
        }
    }

    IEnumerator AnimatePanelOpen() {
        float elapsedTime = 0f;
        float waitTime = 0.3f;
        Vector3 initialScale = Vector3.zero;
        Vector3 finalScale = new Vector3(1.1f, 0.75f, 1.18f);
        ScorePanel.SetActive(true);
        PrintScore();

        while(elapsedTime < waitTime) {
            elapsedTime += Time.unscaledDeltaTime;
            ScorePanel.transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / waitTime);
            yield return null;
        }

    }
    IEnumerator AnimatePanelClose() {
        float elapsedTime = 0f;
        float waitTime = 0.3f;
        Vector3 initialScale = Vector3.zero;
        Vector3 finalScale = new Vector3(1.1f, 0.75f, 1.18f);
        while(elapsedTime < waitTime) {
            elapsedTime += Time.unscaledDeltaTime;
            ScorePanel.transform.localScale = Vector3.Lerp(finalScale, initialScale, elapsedTime / waitTime);
            yield return null;
        }
        ScorePanel.SetActive(false);

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
