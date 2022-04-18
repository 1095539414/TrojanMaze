using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineWriter : MonoBehaviour
{

    private List<Vector3> posiList = new List<Vector3>();
    [SerializeField] LineRenderer lineRender;
    [SerializeField] GameObject player;
    private Vector3 prevPosition;
    // Start is called before the first frame update
    void Start()
    {
        // CreateLine();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPosition = player.transform.position;
        UpdateLine(curPosition);
    }

    private void CreateLine() {
        Vector3 curPosition = player.transform.position;
        lineRender.positionCount++;
        lineRender.SetPosition(0, curPosition);
    }

    private void UpdateLine(Vector3 newPosi) {
        posiList.Add(newPosi);
        lineRender.positionCount++;
        lineRender.SetPosition(lineRender.positionCount - 1, newPosi);
    }

}
