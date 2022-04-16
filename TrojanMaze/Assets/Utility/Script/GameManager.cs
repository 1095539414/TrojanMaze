using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameObject DeathUI;
    public GameObject PassUI;
    public Image PortalImage;
    public Image PortalR;
    public Image PortalT;
    public GameObject PortalDisgard;

    public TextMeshProUGUI BulletUI;
    void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Respawn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void OnLoadNextLevel() {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSecondsRealtime(1f);
        UnityAnalytics.sendLevelSolved();
        UnityAnalytics.sendDamagedFrom();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
        Time.timeScale = 1f;
    }
}
