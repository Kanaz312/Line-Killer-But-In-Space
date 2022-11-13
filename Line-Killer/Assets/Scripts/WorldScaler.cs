using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ScaleWorldEvent(Vector2 center, float scale);

public class WorldScaler : MonoBehaviour
{

    public static event ScaleWorldEvent OnScaleWorld;

    private LineHandler _lineHandler = null;

    private void Start()
    {
        _lineHandler = FindObjectOfType<LineHandler>();
    }

    private void Update()
    {
        ScaleWorld();
    }

    public void ScaleWorld()
    {
        OnScaleWorld?.Invoke(_lineHandler.GetCentroid(), 1);
    }
}
