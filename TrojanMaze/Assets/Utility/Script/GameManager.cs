using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameObject DeathUI;
    public GameObject PassUI;
    public GameObject PortalUI;
    public TextMeshProUGUI BulletUI;
    public FieldOfView FOV;
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
}
