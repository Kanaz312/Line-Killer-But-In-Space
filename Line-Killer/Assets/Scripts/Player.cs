using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    LineRiding,
    Cutting,
    Returning
}

public class Player : MonoBehaviour
{
    [SerializeField] private float _radius = 1.0f;
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _cutSpeed = 3.0f;

    private Camera _cam = null;
    private LineHandler _lineHandler = null;
    private Aimer _aimer = null;
    private Cutter _cutter = null;
    private SpriteRenderer _renderer = null;
    private WorldScaler _worldScaler = null;
    private float _interpVal = 0.0f;
    private int _lineIndex = 0;

    private PlayerState _state = PlayerState.LineRiding;
    private Vector2 _cutStartPoint = new Vector2(0.0f, 0.0f);
    private int _cutStartLine = -1;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _lineHandler = FindObjectOfType<LineHandler>();
        _renderer = GetComponent<SpriteRenderer>();
        _aimer = GetComponent<Aimer>();
        _cutter = GetComponent<Cutter>();
        _worldScaler = FindObjectOfType<WorldScaler>();
        if (_worldScaler == null)
        {
            Debug.LogError("Cutter did not find a WorldScaler object");
        }
        StartCoroutine(ResetPosition());
        WorldScaler.OnScaleWorld += OnScaleWorld;
    }

    private void OnDestroy()
    {
        WorldScaler.OnScaleWorld -= OnScaleWorld;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case PlayerState.LineRiding:
                MoveAlongLines();
                _aimer.AimAndRender();
                RegisterCut();
                break;
            case PlayerState.Cutting:
                MoveForCut();
                break;
            default:
                Debug.LogError("Unknown player state");
                break;
        }

        Color color = _lineHandler.GetIndexCollidingWithCircle((Vector2)transform.position, _radius) > -1
            ? Color.red : Color.green;
        _renderer.color = color;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(_radius, 0.0f, 0.0f));
    }

    void MoveToMouse()
    {
        Vector3 mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = _cam.transform.position.z + _cam.nearClipPlane;
        transform.position = mousePosition;
    }

    void MoveAlongLines()
    {
        float movementRemaining = _speed * Time.deltaTime,
            distance;
        Vector3 currentPosition = _lineHandler.GetPosition(_lineIndex, _interpVal),
            nextPosition = _lineHandler.GetPosition(_lineIndex, 1.0f);
        distance = (nextPosition - currentPosition).magnitude;
        while (distance < movementRemaining)
        {
            movementRemaining -= distance;
            currentPosition = nextPosition;
            _lineIndex = _lineHandler.IncrementPointIndex(_lineIndex);
            nextPosition = _lineHandler.GetPosition(_lineIndex, 1.0f);
            distance = (nextPosition - currentPosition).magnitude;
        }

        currentPosition += (nextPosition - currentPosition).normalized * movementRemaining;
        transform.position = currentPosition;

        _interpVal = _lineHandler.GetInterpValueOnLine(currentPosition, _lineIndex);

        /*_interpVal += _speed;

        if (_interpVal > 1.0f)
        {
            _lineIndex =  _lineHandler.IncrementPointIndex(_lineIndex);
            _interpVal = 0.0f;
        }
        else if (_interpVal < 0.0f)
        {
            _lineIndex = _lineHandler.DecrementPointIndex(_lineIndex);
            _interpVal = 1.0f;
        }

        Vector3 pos = _lineHandler.GetPosition(_lineIndex, _interpVal);
        pos.z = 0.0f;
        transform.position = pos;*/
    }

    void MoveForCut()
    {
        Vector3 pos = transform.position,
            dir = _aimer.GetCutDir();

        pos = pos + dir * _cutSpeed * Time.deltaTime;
        transform.position = pos;

        // Check to stop cut
        int pointIndex = _lineHandler.GetIndexCollidingWithCircle(pos, _radius);
        if (pointIndex > -1 && pointIndex != _cutStartLine)
        {
            _state = PlayerState.LineRiding;

            // Cut Section
            Vector2 cutEndPoint = _lineHandler.GetClosestPointOnLine(pos, pointIndex);
            _cutter.CutSection(_cutStartLine, _cutStartPoint,
                pointIndex, cutEndPoint);
            
            // Set position to new endpoint
            _lineIndex = _lineHandler.GetIndexOfPoint(cutEndPoint);
            _interpVal = _lineHandler.GetInterpValueOnLine(cutEndPoint, _lineIndex);
            _worldScaler.ScaleWorld();
        }
    }

    void RegisterCut()
    {
        Vector3 nextFramePosition = transform.position,
            dir = _aimer.GetCutDir();

        nextFramePosition = nextFramePosition + dir * _cutSpeed * Time.deltaTime;

        if (Input.GetMouseButtonDown(0) 
            && _aimer.CanCut(_lineIndex, _lineHandler)
            && _lineHandler.IsPointInsidePolygon(nextFramePosition))
        {
            _cutStartPoint = transform.position;
            _state = PlayerState.Cutting;
            _cutStartLine = _lineHandler.GetIndexCollidingWithCircle(transform.position, _radius);
        }
    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForEndOfFrame();
        Vector2 pos = _lineHandler.GetInitialPosition();
        transform.position = pos;
        _interpVal = 0.0f;
        _lineIndex = 0;
        _state = PlayerState.LineRiding;
    }

    public void OnScaleWorld(Vector2 point, float scale)
    {
        Vector3 position = transform.position;
        transform.position = position * scale;
    }
}
