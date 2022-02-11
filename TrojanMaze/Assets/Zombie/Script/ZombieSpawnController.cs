using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour {

    [SerializeField] List<Zombie> zombieTypes;
    [SerializeField] List<int> zombieAmount;

    int count;
    // Start is called before the first frame update
    void Start() {
        if(zombieTypes.Count != zombieAmount.Count) {
            Debug.LogWarning("Zombie Spawn Controller: Size of zombieTypes and zombieAmount should match");
            return;
        }
        Debug.Log("Respawning");
        float centerX = transform.position.x;
        float centerY = transform.position.y;
        float radius = GetComponent<Renderer>().bounds.size.x/2;
        for(int i = 0; i < zombieTypes.Count; i++) {
            for(int j = 0; j < zombieAmount[i]; j++) {
                Instantiate(zombieTypes[i], randomPosition(centerX, centerY, radius), Quaternion.identity);
            }
        }
    }

    private Vector3 randomPosition(float x, float y, float r) {
        float deltaX = Random.Range(-r, r);
        float deltaY = Random.Range(-r, r);
        return new Vector3(x+deltaX, y+deltaY, transform.position.z);
    }
    // Update is called once per frame
    void Update() {

    }
}
