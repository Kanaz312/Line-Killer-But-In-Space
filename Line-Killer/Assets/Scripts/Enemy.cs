using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int expAmt;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public int GetExp() {
        return expAmt;
    }

    public void Kill() {
        //Debug.Log("KILL ENEMY");
        Destroy(this.gameObject);
    }
}
