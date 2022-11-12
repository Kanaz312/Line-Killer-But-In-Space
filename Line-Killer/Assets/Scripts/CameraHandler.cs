using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
    private Camera _cam;
    private BoxCollider2D _cameraBox;
    [SerializeField] float zVal = -10f;

    [SerializeField] private LineHandler _lineHandler = null;
    [SerializeField] private float padding = 1f;
    [SerializeField] private float zoomSpd = 0.1f;
    private float camSize;

    // Start is called before the first frame update
    void Start()
    {
        _cam = gameObject.GetComponent<Camera>();
        _cameraBox = _cam.GetComponent<BoxCollider2D>();
        

        _lineHandler = FindObjectOfType<LineHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        Rect polyRect = _lineHandler.GetPointBounds();
        Debug.Log("Width: " + polyRect.width.ToString() + " || Height: " + polyRect.height.ToString()); // + " " + polyRect.yMin.ToString() + " " + polyRect.yMax.ToString());
        //Debug.Log()
        //_cam.rect = new Rect(
        //    polyRect.xMin,
        //    polyRect.yMax,
        //    polyRect.width,
        //    polyRect.height

        //    );


        //    )
       // polyRect.xMin - padding - (polyRect.width/2) + (width/2)
                        
        if (polyRect.width > (polyRect.height * _cam.aspect) + 2 ) {
            camSize = ((polyRect.width / 2) / _cam.aspect) + padding;
        } else { 
            camSize = (polyRect.height / 2) + padding;
        }

        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, camSize, zoomSpd);

        float height = 2f * _cam.orthographicSize;
        float width = height * _cam.aspect;

        transform.position = new Vector3(polyRect.xMin + (polyRect.width / 2), polyRect.yMax - (polyRect.height/2), zVal);
    }
}
