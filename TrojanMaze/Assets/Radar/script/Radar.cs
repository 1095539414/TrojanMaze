using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{

    private Transform pulseTransform;
    private float range;
    private float rangeMax;

    private List<Collider2D> VisitedObj;

    [SerializeField] GameObject EnemyPoint;

    void Start()
    {
        rangeMax = 11f;
        VisitedObj = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float rangeSpeed = 20f;
        range += rangeSpeed * Time.deltaTime;
        if(range > rangeMax) {
            range = 0f;
            VisitedObj.Clear();
        }
        transform.localScale = new Vector3(range, range);

        RaycastHit2D[] array = Physics2D.CircleCastAll(transform.position, range /2f, Vector2.zero);
        foreach (RaycastHit2D rayHit in array) {
            if(rayHit.collider.tag == "Zombie" && !VisitedObj.Contains(rayHit.collider)) {
                VisitedObj.Add(rayHit.collider);
                // rayHit.collider.Destroy();
                Debug.Log("Hit");
                Instantiate(EnemyPoint, rayHit.collider.transform.position, rayHit.collider.transform.rotation);
            }
        }
    }
}
