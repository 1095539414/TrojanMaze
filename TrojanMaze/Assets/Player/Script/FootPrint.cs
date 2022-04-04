using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrint : MonoBehaviour
{
    [SerializeField] float ValidityPeriod = 20f;
    [SerializeField] float FadedPeriod = 10f;
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
        if(_time > ValidityPeriod) {
            Destroy(gameObject);
        } else if(_time > FadedPeriod) {
            alpha = 1 - (_time - FadedPeriod) / (ValidityPeriod - FadedPeriod);
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
