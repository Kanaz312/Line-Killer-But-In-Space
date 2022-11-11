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

    private Camera _cam = null;
    private LineHandler _lineHandler = null;
    private SpriteRenderer _renderer = null;
    private float _interpVal = 0.0f;
    private int _lineIndex = 0;

    private PlayerState _state = PlayerState.LineRiding;
    private Vector2 _cutStartPoint = new Vector2(0.0f, 0.0f);
    private Vector2 _cutDir = new Vector2(1.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _lineHandler = FindObjectOfType<LineHandler>();
        _renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ResetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case PlayerState.LineRiding:
                MoveAlongLines();
                break;
            case PlayerState.Cutting:
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
        if (Input.GetKey(KeyCode.W))
        {
            _interpVal += _speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _interpVal -= _speed;
        }

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

        transform.position = _lineHandler.GetPosition(_lineIndex, _interpVal);
    }

    void MoveForCut()
    {

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
}
