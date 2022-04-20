using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrailIcon : MonoBehaviour
{
    private GameObject trailMapIcon;
    private float time = 0;
    [SerializeField] float liveTime = 5f;
    public GameObject trailPanel;

    // Start is called before the first frame update
    void Start()
    {
        trailMapIcon = transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(trailMapIcon != null) {
            if(State.getKeyState()) {
                time += Time.deltaTime;
                trailMapIcon.GetComponent<Renderer>().enabled = true;
                if(time  > liveTime) {
                    State.setKeyState(false);
                    time = 0;
                    if(trailPanel != null) {
                        trailPanel.SetActive(false);
                    }
                }
            } else {
                trailMapIcon.GetComponent<Renderer>().enabled = false;
                time = 0;
            }
        }

    }
}
