using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ScaleWorldEvent(Vector2 center, float scale);

public class WorldScaler : MonoBehaviour
{

    public static event ScaleWorldEvent OnScaleWorld;

    private float _originalSideLength = 1.0f;

    private LineHandler _lineHandler = null;
    private CameraHandler _cameraHandler = null;

    private void Start()
    {
        _lineHandler = FindObjectOfType<LineHandler>();
        _cameraHandler = FindObjectOfType<CameraHandler>();

        // Get the side length (assumes play area is square)
        Rect playAreaBounds = _lineHandler.GetPointBounds();
        _originalSideLength = playAreaBounds.height;
    }

    public void ScaleWorld()
    {
        Rect playAreaBounds = _lineHandler.GetPointBounds();
        float minSide = Mathf.Min(playAreaBounds.width, playAreaBounds.height);

        float scale = _originalSideLength / minSide;
        OnScaleWorld?.Invoke(_lineHandler.GetCentroid(), scale);
    }
}
