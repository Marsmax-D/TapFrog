using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Transform frog;
    public float offsetY;
    public float zoomBase;
    private float ration;

    private void Start()
    {
        ration = (float)Screen.height / (float)Screen.width;
        Camera.main.orthographicSize = zoomBase * ration * 0.5f;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, frog.transform.position.y + offsetY * ration, transform.position.z);
    }
}
