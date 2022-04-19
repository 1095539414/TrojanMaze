using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour {
    [SerializeField] float levelLoadDelay = 1f;
    public GameObject StartButtonPressed;
    public GameObject StartButton;
    private string levelName = "No2_demo1";

    private void Start() {
        if(StartButton)
            StartButton.SetActive(true);
        if(StartButtonPressed)
            StartButtonPressed.SetActive(false);
    }
    public void PlayIntro() {
        StartCoroutine(LoadScene("Intro"));
    }
    public void PlayLevel1() {
        StartCoroutine(LoadScene("Maze1"));
    }

    public void PlayLevel2() {
        StartCoroutine(LoadScene("Maze1.2"));
    }

    public void QuitScene() {
        Time.timeScale = 1f;
        StartCoroutine(LoadScene("menu"));
    }

    public void OnStartClick() {
        StartCoroutine(AnimateStartButton());
        StartCoroutine(LoadScene(levelName));
    }

    IEnumerator AnimateStartButton() {
        StartButton.SetActive(false);
        StartButtonPressed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        StartButton.SetActive(true);
        StartButtonPressed.SetActive(false);
    }

    IEnumerator LoadScene(string sceneName) {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
