using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {
    [SerializeField] float levelLoadDelay = 1f;

    void Start(){
        GameManager.instance.PassUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Time.timeScale = 0f;
            GameManager.instance.PassUI.SetActive(true);
        }
    }


    public void OnLoadNextLevel(){
        Time.timeScale = 1f;
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        UnityAnalytics.sendLevelSolved();
        UnityAnalytics.sendDamagedFrom();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void OnApplicationQuit() {
        Time.timeScale = 1f;
        UnityAnalytics.sendDamagedFrom();
    }

    public void OnRestart(){
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

}
