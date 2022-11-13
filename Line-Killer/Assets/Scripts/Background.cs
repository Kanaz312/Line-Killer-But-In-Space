using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private CameraHandler _camHandler;
    private Camera _cam;
    // Start is called before the first frame update
    void Start()
    {
        _camHandler = FindObjectOfType<CameraHandler>();
        _cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        Vector3 pos = _cam.transform.position;
        pos.z = 1;
        transform.position = pos;
        Vector2 cameraDimensions = _camHandler.GetScreenDimensions();
        transform.localScale = new Vector3(cameraDimensions.x, cameraDimensions.y, 1.0f);
    }
}
