using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    public Image[] slots;
    private GameObject[] inventories;
    public static InventoryManager instance;
    public Color color;

    private void Awake() {
        instance = this;
        inventories = new GameObject[3];
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            UseItem(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            UseItem(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            UseItem(2);
        }
    }

    public bool AddItem(GameObject item) {
        for(int i = 0; i < inventories.Length; i++) {
            if(inventories[i] == null) {
                inventories[i] = item;
                item.gameObject.SetActive(false);
                slots[i].sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
                slots[i].color = Color.white;
                slots[i].gameObject.GetComponent<TooltipTrigger>().SetText(
                    item.GetComponent<BuffItem>().content,
                    item.GetComponent<BuffItem>().header
                );
                return true;
            }
        }
        return false;
    }

    public void UseItem(int index) {
        if(index >= inventories.Length) {
            return;
        }

        if(inventories[index] == null) {
            return;
        }
        inventories[index].GetComponent<BuffItem>().Activate();
        inventories[index] = null;
        slots[index].sprite = null;
        slots[index].color = color;
        slots[index].gameObject.GetComponent<TooltipTrigger>().Reset();
        return;
    }
}
