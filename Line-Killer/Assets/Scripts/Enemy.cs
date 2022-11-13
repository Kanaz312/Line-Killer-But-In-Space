using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _deadBodyPrefab = null;
    protected int expAmt;
    // Start is called before the first frame update
    void Start() {
        WorldScaler.OnScaleWorld += OnScaleWorld;
    }

    private void OnDestroy() {
        WorldScaler.OnScaleWorld -= OnScaleWorld;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public int GetExp() {
        return expAmt;
    }

    public void Kill() {
        //Debug.Log("KILL ENEMY");
        GameObject body = Instantiate(_deadBodyPrefab);
        body.transform.position = transform.position;
        Destroy(this.gameObject);
    }
    public void OnScaleWorld(Vector2 pt, float scale) {
        transform.position = transform.position * scale;
    }
}
