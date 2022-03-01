using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class Exit : MonoBehaviour {
    [SerializeField] float levelLoadDelay = 1f;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            StartCoroutine(LoadNextLevel());
        }
    }
    IEnumerator LoadNextLevel() {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        AnalyticsResult analyticsResult = Analytics.CustomEvent(
            "LevelSolved",
            new Dictionary<string, object>{
                {"Level Name", SceneManager.GetActiveScene().name},
                {"Time", Time.timeSinceLevelLoad},
                {"Remaining HP", Move.GetHP()},
                {"Received Damage", Move.totalHpReduced},
                {"Gun Damage", Move.dmgByGun},
                {"Sword Damage", Move.dmgBySword}
            }
        );

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
