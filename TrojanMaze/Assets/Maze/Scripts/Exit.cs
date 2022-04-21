using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {
    [SerializeField] float levelLoadDelay = 1f;

    void Start() {
        GameManager.instance.PassUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        GameObject[] hint = GameManager.instance.nextLevelHints;
        if(other.tag == "Player") {
            Time.timeScale = 0f;
            GameManager.instance.PassUI.SetActive(true);
            for(int i = 0; i < hint.Length; i++) {
                if(i != SceneManager.GetActiveScene().buildIndex) {
                    hint[i].SetActive(false);
                } else {
                    hint[i].SetActive(true);
                }
            }
        }
    }


    public void OnApplicationQuit() {
        Time.timeScale = 1f;
        UnityAnalytics.sendDamagedFrom();
    }

    public void OnRestart() {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

}
