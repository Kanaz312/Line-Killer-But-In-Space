using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHandler : MonoBehaviour
{
    [SerializeField] private float _width = 1.0f;
    [SerializeField] private Color _color = Color.green;
    [SerializeField] private List<Vector3> _points = null;
    private LineRenderer _lineRenderer = null;
    
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // Render Lines
        _lineRenderer.startColor = _color;
        _lineRenderer.endColor = _color;
        _lineRenderer.startWidth = _width;
        _lineRenderer.endWidth = _width;
        _lineRenderer.positionCount = _points.Count;
        _lineRenderer.SetPositions(_points.ToArray());
    }

    public bool IsCircleCollidingLine(Vector2 center, float radius,
        Vector2 pointA, Vector2 pointB)
    {
        // Get vectors from one point to the other
        Vector2 AToB = pointB - pointA,
            BToA = pointA - pointB;


        // Get vectors froms points to center
        Vector2 AToCenter = center - pointA,
            BToCenter = center - pointB;

        if (Vector2.Dot(AToB.normalized, AToCenter.normalized) >= 0.0f
            && Vector2.Dot(BToA.normalized, BToCenter.normalized) >= 0.0f)
        {
            // Get normal of line
            Vector2 normal = new Vector2(-(pointB.y - pointA.y), pointB.x - pointA.x);
            normal.Normalize();

            if (Mathf.Abs(Vector2.Dot(AToCenter, normal)) < radius)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsCircleCollidingAnyLine(Vector2 center, float radius)
    {
        for (int i = 0; i < _points.Count - 1; i++)
        {
            // Get points of line
            Vector2 pointA = _points[i],
                pointB = _points[i + 1];

            if (IsCircleCollidingLine(center, radius, pointA, pointB))
            {
                return true;
            }
        }
        if (IsCircleCollidingLine(center, radius,
            _points[_points.Count - 1], _points[0]))
        {
            return true;
        }
        return false;
    }

    public int GetIndexCollidingWithCircle(Vector2 center, float radius)
    {
        for (int i = 0; i < _points.Count - 1; i++)
        {
            // Get points of line
            Vector2 pointA = _points[i],
                pointB = _points[i + 1];

            if (IsCircleCollidingLine(center, radius, pointA, pointB))
            {
                return i;
            }
        }
        if (IsCircleCollidingLine(center, radius,
            _points[_points.Count - 1], _points[0]))
        {
            return _points.Count - 1;
        }
        return -1;
    }

    public Vector2 GetInitialPosition()
    {
        return (Vector2)_points[0];
    }

    public int IncrementPointIndex(int index)
    {
        return (index + 1) % _points.Count;
    }

    public int DecrementPointIndex(int index)
    {
        return (index - 1) < 0 ? _points.Count - 1 : index - 1;
    }

    public Vector2 GetPosition(int index, float t)
    {
        Vector2 ret;
        Vector3 pointA, pointB;
        
        if (index == _points.Count - 1)
        {
            pointA = _points[index];
            pointB = _points[0];

        }
        else
        {
            pointA = _points[index];
            pointB = _points[index + 1];
        }

        ret.x = Mathf.Lerp(pointA.x, pointB.x, t);
        ret.y = Mathf.Lerp(pointA.y, pointB.y, t);
        return ret;
    }

    public Rect GetPointBounds() {
        List<Vector3> pts = _points;
        float xMin = pts[0].x;
        float xMax = pts[0].x;
        float yMin = pts[0].y;
        float yMax = pts[0].y;
        // xMax = yMax = float.MinValue;
        // xMin = yMin = float.MaxValue;
        foreach (Vector3 p in pts) {
            if (p.x < xMin) {
                xMin = p.x;
            }
            if (p.x > xMax) {
                xMax = p.x;
            }
            if (p.y < yMin) {
                yMin = p.y;
            }
            if (p.y > yMax) {
                yMax = p.y;
            }
        }

        Rect r = new Rect(xMin, yMin, (xMax - xMin), (yMax - yMin));
        //r.xMax = xMax;
        //r.yMax = yMax;
        return r;
    }
}
