using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();
    private int seconds = 0;
    private int nextUpdate = 1;

    private LineHandler _lineHandler = null;

    public Enemy enemy;

    enum EnemyType {
        Chiller,
        Turrets,
        Chasers,
        Swivler
    }

    //protected int[] ExpAmt = new int[] { 10, 25, 35, 20 };    
    // Start is called before the first frame update
    void Start() {
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
        switch (type) {
            case EnemyType.Chiller:
                Rect polyPts = _lineHandler.GetPointBounds();
                Vector2 v2 = new Vector2(Random.Range(polyPts.xMin, polyPts.xMax), Random.Range(polyPts.yMin, polyPts.yMax));

                while (!_lineHandler.IsPointInsidePolygon(v2)) {
                    v2 = new Vector2(Random.Range(polyPts.xMin, polyPts.xMax), Random.Range(polyPts.yMin, polyPts.yMax));
                }

                Enemy e = Instantiate(enemy, v2, Quaternion.Euler(new Vector3(0,0,0)));//new Vector3(enemy.transform.position.x + Random.Range(-10, 10), enemy.transform.position.y);
                enemies.Add(e);
                break;
        }
    }

    public void KillOutOfBounds() {
        foreach (Enemy e in enemies.ToArray()) {
            Debug.Log(_lineHandler.IsPointInsidePolygon(e.transform.position));
            if (!_lineHandler.IsPointInsidePolygon(e.transform.position)) {
                enemies.Remove(e);
                e.Kill();
            }
        }
        
    }
}
