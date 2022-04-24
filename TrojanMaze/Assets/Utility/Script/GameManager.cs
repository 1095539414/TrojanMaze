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
    public Camera MainCamera;

    public TextMeshProUGUI BulletUI;

    public GameObject BulletImg;

    public TextMeshProUGUI TimeSpent;

    public float itemDropRate = 0f;

    public BuffItem[] itemsToDrop;
    public GameObject[] nextLevelHints;

    public GameObject PassContinue;
    public GameObject PassReturn;
    public GameObject PassContinuePressed;
    public GameObject PassReturnPressed;

    public TextMeshProUGUI ScoreTextUI;
    public GameObject DeathRestart;
    public GameObject DeathReturn;
    public GameObject DeathRestartPressed;
    public GameObject DeathReturnPressed;
    public GameObject Quit;
    public GameObject QuitPressed;

    public GameObject ComingUp;

    void Awake() {
        instance = this;
        PortalObject.SetActive(SceneManager.GetActiveScene().buildIndex >= 3);
        RadarObject.SetActive(SceneManager.GetActiveScene().buildIndex >= 4);
        TrailmapObject.SetActive(SceneManager.GetActiveScene().buildIndex >= 5);
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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

    public void OnContinueClicked() {
        StartCoroutine(AnimateContinueButton());
        StartCoroutine(LoadNextLevel());

    }
    public void OnReturnClicked() {
        Time.timeScale = 1f;
        StartCoroutine(AnimateReturnButton());
        SceneManager.LoadScene((int)BuildIndex.MENU);
    }

    public void OnQuitClicked() {
        Time.timeScale = 1f;
        StartCoroutine(AnimateQuitButton());
        SceneManager.LoadScene((int)BuildIndex.MENU);
    }


    IEnumerator AnimateContinueButton() {
        PassContinue.SetActive(false);
        PassContinuePressed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        PassContinue.SetActive(true);
        PassContinuePressed.SetActive(false);
    }

    IEnumerator AnimateReturnButton() {
        PassReturn.SetActive(false);
        PassReturnPressed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        PassReturn.SetActive(true);
        PassReturnPressed.SetActive(false);
    }

    IEnumerator AnimateQuitButton() {
        Quit.SetActive(false);
        QuitPressed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Quit.SetActive(true);
        QuitPressed.SetActive(false);
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
