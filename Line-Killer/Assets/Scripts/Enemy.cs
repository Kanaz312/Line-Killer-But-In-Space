using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Kill() {
        //Debug.Log("KILL ENEMY");
        Destroy(this.gameObject);
    }
}
