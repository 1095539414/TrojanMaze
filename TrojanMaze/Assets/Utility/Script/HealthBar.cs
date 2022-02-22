using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider slider;

    void SetHealth(float health){
        slider.value = health;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetHealth(Move.GetHP());
    }
}
