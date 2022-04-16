using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoint : MonoBehaviour
{

    [SerializeField] float FadedPeriod = 1f;
    private float _time = 0f;
    private GameObject currentGameObject;
    private float alpha = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        currentGameObject = gameObject;  
    }

    // Update is called once per frame
    void Update()
    {
         _time += Time.deltaTime;
        // ValidityPeriod must be larger than FadedPeriod
        if(_time >= FadedPeriod) {
            Destroy(gameObject);
        } else  {
            alpha = (FadedPeriod - _time) / FadedPeriod;
            ChangeOpacity(currentGameObject.GetComponent<Renderer>().material, alpha);
        }
    }

    private void ChangeOpacity(Material mat, float alpha)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        mat.SetColor("_Color", newColor);
    }
}
