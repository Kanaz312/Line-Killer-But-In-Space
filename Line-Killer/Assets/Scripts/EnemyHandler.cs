using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();
    private Player _player = null;
    private int seconds = 0;
    private int nextUpdate = 1;

    private LineHandler _lineHandler = null;

    public Enemy enemy;
    public Chiller chiller;

    enum EnemyType {
        Chiller,
        Turrets,
        Chasers,
        Swivler
    }

    //protected int[] ExpAmt = new int[] { 10, 25, 35, 20 };    
    // Start is called before the first frame update
    void Start() {
        _player = FindObjectOfType<Player>();
        _lineHandler = FindObjectOfType<LineHandler>();
    }

    // Update is called once per frame
    void Update() {

        // If the next update is reached
        if (Time.time >= nextUpdate) {
            //Debug.Log(Time.time + ">=" + nextUpdate);
            seconds++;
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            UpdateEverySecond();
        }

    }

    void UpdateEverySecond() {
        if (seconds % 5 == 0) {
            SpawnEnemy(EnemyType.Chiller);
        }
        
    }


    void SpawnEnemy(EnemyType type) {
        Rect polyPts = _lineHandler.GetPointBounds();

        // Keep making new vectors until valid point :)
        Vector2 v2 = new Vector2(Random.Range(polyPts.xMin, polyPts.xMax), Random.Range(polyPts.yMin, polyPts.yMax));
        Enemy e = null;
        while (!_lineHandler.IsPointInsidePolygon(v2)) {
            v2 = new Vector2(Random.Range(polyPts.xMin, polyPts.xMax), Random.Range(polyPts.yMin, polyPts.yMax));
        }

        switch (type) {
            case EnemyType.Chiller:
                
                // Instantiate enemy, add to list
                e = Instantiate(chiller, v2, Quaternion.Euler(new Vector3(0,0,0)));//new Vector3(enemy.transform.position.x + Random.Range(-10, 10), enemy.transform.position.y);
                
                break;
        }

        if (e != null) {
            enemies.Add(e);
        }

    }

    public void KillOutOfBounds() {
        // Loop through all enemies
        foreach (Enemy e in enemies.ToArray()) {
            if (!_lineHandler.IsPointInsidePolygon(e.transform.position)) {
                // Give Player EXP
                _player.GetComponent<Level>().GainExp(e.GetExp());
                // Kill Enemy
                enemies.Remove(e);
                e.Kill();

                Debug.Log(_player.GetComponent<Level>().GetExp());
            }
        }
        
    }
}
