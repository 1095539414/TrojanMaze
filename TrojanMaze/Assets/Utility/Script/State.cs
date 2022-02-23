using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {
    
    [SerializeField]
    private BuffItem buffPrefab;

    [SerializeField]
    private Transform buffTransform;


    private List<BuffItem> buffs = new List<BuffItem>();

    public void AddBuff(SpriteRenderer spriteR,string name){
        BuffItem bf = Instantiate(buffPrefab, buffTransform);
        bf.Initialize(spriteR,name);
        buffs.Add(bf);
    }

    public void RemoveBuff(string name){
         BuffItem bf = buffs.Find(x => x.name == name);
         buffs.Remove(bf);
         Destroy(bf.gameObject);
    }
}
