using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class State : MonoBehaviour {

    [SerializeField]
    private BuffItem buffPrefab;

    [SerializeField]
    private Transform buffTransform;

    private static bool isOnTower = false;
    private static bool isGetKey = false;


    private List<BuffItem> buffs = new List<BuffItem>();

    public void AddBuff(string name, Sprite sprite) {
        BuffItem bf = Instantiate(buffPrefab, buffTransform);
        bf.Initialize(name, sprite);
        buffs.Add(bf);
    }

    public void RemoveBuff(string name) {
        BuffItem bf = buffs.Find(x => x.name == name);
        buffs.Remove(bf);
        Destroy(bf.gameObject);
    }

    public static void SetTowerState(bool state) {
        isOnTower = state;
    }

    public static bool getTowerState() {
        return isOnTower;
    }

    public static void setKeyState(bool state) {
        isGetKey = state;
    }

    public static bool getKeyState() {
        return isGetKey;
    }
}
