using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    [SerializeField] private float _aimLength = 2.0f;
    [SerializeField] private float _aimWidth = 1.0f;

    private Camera _cam = null;
    private LineRenderer _aimRenderer = null;
    private Vector2 _cutDir = new Vector2(1.0f, 0.0f);


    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _aimRenderer = GetComponent<LineRenderer>();
        _aimRenderer.positionCount = 2;
    }

    public void AimAndRender()
    {
        Aim();
        RenderAim();
    }

    void Aim()
    {
        Vector3 mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.0f;
        _cutDir = (mousePosition - transform.position).normalized;
    }

    void RenderAim()
    {
        Vector3 normalizedCutDireciton = (Vector3)_cutDir;
        normalizedCutDireciton.z = 0;
        Vector3 posA = transform.position,
            posB = transform.position + normalizedCutDireciton * _aimLength;
        posA.z = -2;
        posB.z = -2;
        _aimRenderer.SetPosition(0, posA);
        _aimRenderer.SetPosition(1, posB);
        _aimRenderer.startWidth = _aimWidth;
        _aimRenderer.endWidth = _aimWidth;
    }

    public Vector2 GetCutDir()
    {
        return _cutDir;
    }

    public bool CanCut(int lineIndex, LineHandler lineHandler)
    {
        Vector2 normal = lineHandler.GetInwardNormalOfLine(lineIndex);
        float dotProd = Vector2.Dot(normal, _cutDir);
        return dotProd > 0.0f;
    }
}
