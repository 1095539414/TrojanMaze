using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthBar : MonoBehaviour
{
    // Start is called before the first frame update

   [SerializeField] Transform bar;
    void Start()
    {
        bar.localScale = new Vector3(1f, 1f);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    // }

    public void SetBar(float curHealth, float maxHealth) {
        bar.localScale = new Vector3(curHealth / maxHealth, 1f);
        Debug.Log("hit!!!!" + curHealth / maxHealth);
    }
}
