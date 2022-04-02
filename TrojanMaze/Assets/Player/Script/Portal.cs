using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Vector2 _location;
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _location = transform.position;
        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("Click this Protal");
        // this object was clicked - do something
        Destroy(this.gameObject);
        _player.transform.position = _location;
    }   
}
