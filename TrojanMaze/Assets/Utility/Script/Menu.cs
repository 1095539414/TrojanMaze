using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Menu : MonoBehaviour {
    public GameObject StartButtonPressed;
    public GameObject StartButton;
    public GameObject DarkModeButton;
    public GameObject DarkModeButtonPressed;
    public GameObject[] HintGameObject;
    public RenderPipelineAsset lightRenderer;
    public RenderPipelineAsset darkRenderer;
    private int levelIndex = -1;

    private void Start() {
        if(StartButton) {
            StartButton.SetActive(true);
        }
        if(StartButtonPressed) {
            StartButtonPressed.SetActive(false);
        }
        if(DarkModeButton) {
            DarkModeButton.SetActive(true);
        }
        if(DarkModeButtonPressed) {
            DarkModeButtonPressed.SetActive(false);
        }
        EnableLightMode();
    }

    public void OnStartClick() {
        StartCoroutine(AnimateStartButton());
        StartCoroutine(LoadScene(levelIndex));
    }
    public void OnDarkModeClick() {
        EnableDarkMode();
        DarkModeButton.SetActive(false);
        DarkModeButtonPressed.SetActive(true);
    }

    public void OnDarkModePressedClick() {
        EnableLightMode();
        DarkModeButton.SetActive(true);
        DarkModeButtonPressed.SetActive(false);
    }

    IEnumerator AnimateStartButton() {
        StartButton.SetActive(false);
        StartButtonPressed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        StartButton.SetActive(true);
        StartButtonPressed.SetActive(false);
    }

    IEnumerator LoadScene(int sceneIndex) {
        if(sceneIndex > -1) {
            yield return new WaitForSecondsRealtime(0.5f);
            SceneManager.LoadScene(sceneIndex);
        }

    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SetLevelIndex(int index) {
        this.levelIndex = index;
        for(int i = 0; i < HintGameObject.Length; i++) {
            if(i != index - 1) {
                HintGameObject[i].SetActive(false);
            } else {
                HintGameObject[i].SetActive(true);
            }
        }
    }

    public void EnableLightMode() {
        GraphicsSettings.renderPipelineAsset = lightRenderer;
    }
    public void EnableDarkMode() {
        GraphicsSettings.renderPipelineAsset = darkRenderer;
    }
}
