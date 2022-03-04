using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    public void PlayIntro(){
        StartCoroutine(loadScene("Intro"));
    }
    public void PlayLevel1(){
        StartCoroutine(loadScene("Maze1"));
    }

    public void PlayLevel2(){
        StartCoroutine(loadScene("Maze1.2"));
    }

    public void QuitScene(){
        Time.timeScale = 1f;
        StartCoroutine(loadScene("menu"));
    }

    IEnumerator loadScene(string sceneName){
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
