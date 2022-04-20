using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour {
    [SerializeField] float levelLoadDelay = 1f;
    public GameObject StartButtonPressed;
    public GameObject StartButton;
    private int levelIndex = -1;

    private void Start() {
        if(StartButton)
            StartButton.SetActive(true);
        if(StartButtonPressed)
            StartButtonPressed.SetActive(false);
    }
    public void PlayIntro() {
        StartCoroutine(LoadScene(1));
    }
    public void PlayLevel1() {
        StartCoroutine(LoadScene(4));
    }

    public void PlayLevel2() {
        StartCoroutine(LoadScene(5));
    }

    public void QuitScene() {
        Time.timeScale = 1f;
        StartCoroutine(LoadScene(0));
    }

    public void OnStartClick() {
        StartCoroutine(AnimateStartButton());
        StartCoroutine(LoadScene(levelIndex));
    }

    IEnumerator AnimateStartButton() {
        StartButton.SetActive(false);
        StartButtonPressed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        StartButton.SetActive(true);
        StartButtonPressed.SetActive(false);
    }

    IEnumerator LoadScene(int sceneIndex) {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SetLevelIndex(int index) {
        this.levelIndex = index;
    }
}
