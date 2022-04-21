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

    public float itemDropRate = 0f;
    public BuffItem[] itemsToDrop;
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

    public Vector2 GetParabola(Vector2 start, Vector2 v0, float time, float gravity = -9.8f) {
        Vector2 pos = start;
        pos.x += v0.x * time;
        pos.y += v0.y * time + gravity * 0.5f * time * time;
        return pos;
    }
    public void StartAnimateDropItem(BuffItem item) {
        StartCoroutine(AnimateDropItem(item));
    }
    IEnumerator AnimateDropItem(BuffItem item) {
        float elapsedTime = 0f;
        float waitTime = 0.3f;
        Vector2 startPos = item.gameObject.transform.position;
        Vector2 v0 = new Vector2(0.5f, 1f);
        while(elapsedTime < waitTime) {
            elapsedTime += Time.deltaTime;
            item.gameObject.transform.position = GetParabola(startPos, v0, elapsedTime);
            yield return null;

        }
    }
}
