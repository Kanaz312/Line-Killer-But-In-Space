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
        _aimRenderer.SetPosition(0, transform.position);
        _aimRenderer.SetPosition(1, transform.position + normalizedCutDireciton * _aimLength);
        _aimRenderer.startWidth = _aimWidth;
        _aimRenderer.endWidth = _aimWidth;
    }

    public Vector2 GetCutDir()
    {
        return _cutDir;
    }
}
