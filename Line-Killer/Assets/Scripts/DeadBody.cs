using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    [SerializeField] private float _dissolveSpeed = 1.0f;

    private float _dissolveAmount = 0.0f;
    private Renderer _renderer = null;
    private MaterialPropertyBlock _mpb = null;
    // Start is called before the first frame update
    void Start()
    {
        WorldScaler.OnScaleWorld += OnScaleWorld;
        _renderer = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_mpb);
    }

    private void OnDestroy()
    {
        WorldScaler.OnScaleWorld -= OnScaleWorld;
    }

    // Update is called once per frame
    void Update()
    {
        _dissolveAmount += _dissolveSpeed * Time.deltaTime;
        _mpb.SetFloat("_DissolveAmount", _dissolveAmount);
        _renderer.SetPropertyBlock(_mpb);
        if (_dissolveAmount > 1.0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnScaleWorld(Vector2 pt, float scale)
    {
        transform.position = transform.position * scale;
    }
}
