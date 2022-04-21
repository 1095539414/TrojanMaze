using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

    void Start() {
        GameManager.instance.PassUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        GameObject[] hint = GameManager.instance.nextLevelHints;
        if(other.tag == "Player") {
            Time.timeScale = 0f;
            GameManager.instance.PassUI.SetActive(true);
            GameManager.instance.ComingUp.SetActive(
                SceneManager.GetActiveScene().buildIndex == hint.Length
            );
            for(int i = 0; i < hint.Length; i++) {
                hint[i].SetActive(i == SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void OnApplicationQuit() {
        Time.timeScale = 1f;
        UnityAnalytics.sendDamagedFrom();
    }
}
