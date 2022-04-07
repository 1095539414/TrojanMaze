using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    private Vector2 _location;
    private GameObject _player;
    // Start is called before the first frame update
    void Start() {
        _location = transform.position;
    }

    // Update is called once per frame
    void Update() {

    }

    void OnMouseDown() {
    }

    public Vector2 GetLocation() {
        return _location;
    }
}
