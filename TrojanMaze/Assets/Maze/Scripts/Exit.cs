using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

    public GameObject child1;
    public GameObject child2;

    void Start() {
        GameManager.instance.PassUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        GameObject[] hint = GameManager.instance.nextLevelHints;
        if(other.tag == "Player" && other.GetComponent<SpriteRenderer>().isVisible) {
            Time.timeScale = 0f;
            SoundManager.PlaySound("success");
            GameManager.instance.PassUI.SetActive(true);
            GameManager.instance.ComingUp.SetActive(
                SceneManager.GetActiveScene().buildIndex < hint.Length - 1
            );
            GameManager.instance.PassContinue.SetActive(
                SceneManager.GetActiveScene().buildIndex < hint.Length - 1
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

    public void EnableTowerTrigger() {
        child1.GetComponent<TowerEventReceiver>().enabled = true;
        child2.GetComponent<TowerEventReceiver>().enabled = true;
    }
}
