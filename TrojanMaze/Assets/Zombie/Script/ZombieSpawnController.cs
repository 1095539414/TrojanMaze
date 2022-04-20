using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour {

    [SerializeField] List<Zombie> zombieTypes;
    [SerializeField] List<int> zombieAmount;
    private SpriteRenderer renderer;
    int count;
    // Start is called before the first frame update
    void Start() {
        renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        color.a = 0;
        renderer.color = color;
        if(zombieTypes.Count != zombieAmount.Count) {
            Debug.LogWarning("Zombie Spawn Controller: Size of zombieTypes and zombieAmount should match");
            return;
        }
        float centerX = transform.position.x;
        float centerY = transform.position.y;
        float length = GetComponent<Renderer>().bounds.size.x/2;
        float width = GetComponent<Renderer>().bounds.size.y/2;
        for(int i = 0; i < zombieTypes.Count; i++) {
            for(int j = 0; j < zombieAmount[i]; j++) {
                Instantiate(zombieTypes[i], randomPosition(centerX, centerY, length, width), Quaternion.identity);
            }
        }
    }

    private Vector3 randomPosition(float x, float y, float l, float w) {
        float deltaX = Random.Range(-l, l);
        float deltaY = Random.Range(-w, w);
        return new Vector3(x+deltaX, y+deltaY, transform.position.z);
    }
    // Update is called once per frame
    void Update() {

    }
}
