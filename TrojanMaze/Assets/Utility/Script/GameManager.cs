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
    public GameObject PortalObject;
    public GameObject TrailmapObject;
    public GameObject RadarObject;
    public Image PortalImage;
    public Image PortalR;
    public Image PortalT;
    public GameObject PortalDisgard;

    public TextMeshProUGUI BulletUI;

    public float itemDropRate = 0f;

    public BuffItem[] itemsToDrop;
    public GameObject[] nextLevelHints;

    public GameObject Continue;
    public GameObject Return;
    public GameObject ContinuePressed;
    public GameObject ReturnPressed;


    void Awake() {
        instance = this;
        PortalObject.SetActive(SceneManager.GetActiveScene().buildIndex >= 3);
        RadarObject.SetActive(SceneManager.GetActiveScene().buildIndex >= 4);
        TrailmapObject.SetActive(SceneManager.GetActiveScene().buildIndex >= 5);

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

    public void OnContinueClicked() {
        StartCoroutine(AnimateContinueButton());
    }
    public void OnReturnClicked() {
        StartCoroutine(AnimateReturnButton());
    }

    public void QuitScene() {
        Time.timeScale = 1f;
        SceneManager.LoadScene((int)BuildIndex.MENU);
    }

    IEnumerator AnimateContinueButton() {
        Continue.SetActive(false);
        ContinuePressed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Continue.SetActive(true);
        ContinuePressed.SetActive(false);
    }

    IEnumerator AnimateReturnButton() {
        Return.SetActive(false);
        ReturnPressed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Return.SetActive(true);
        ReturnPressed.SetActive(false);
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
