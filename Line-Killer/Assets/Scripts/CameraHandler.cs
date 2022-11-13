using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
    private Camera _cam;
    private LineHandler _lineHandler = null;

    [SerializeField] float zVal = -10f; // The z position of the camera
    [SerializeField] private float padding = 1f;    // The padding around the polygon
    [SerializeField] private float zoomSpd = 0.1f;  // How fast it lerps
    private float targetScale;

    // Start is called before the first frame update
    void Start()
    {
        _cam = gameObject.GetComponent<Camera>();
        _lineHandler = FindObjectOfType<LineHandler>();
        targetScale = _cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Make this a co-routine that's only run when a slice is called
        ScaleScreen();
    }


    // Scales the screen when called
    private void ScaleScreen() {
        // Get the bounds of the polygon
        Rect polyRect = _lineHandler.GetPointBounds();

        //Debug.Log("Width: " + polyRect.width.ToString() + " || Height: " + polyRect.height.ToString()); // + " " + polyRect.yMin.ToString() + " " + polyRect.yMax.ToString());
        float camSize;
        // Scale whether it's wider or smaller with respect to the aspect ratio
        if (polyRect.width > (polyRect.height * _cam.aspect) + padding) {
            camSize = ((polyRect.width / 2) / _cam.aspect) + padding;
        } else {
            camSize = (polyRect.height / 2) + padding;
        }

        // Smooth the camera scaling
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, camSize, zoomSpd);

        // Center the polygon
        Vector3 camPos = new Vector3(polyRect.xMin + (polyRect.width / 2), polyRect.yMax - (polyRect.height / 2), zVal);
        // Smooth the camera transform
        transform.position = Vector3.Lerp(transform.position, camPos, zoomSpd);
    }

    public float GetWorldRescaleFactor()
    {
        return targetScale / _cam.orthographicSize;
    }
}
