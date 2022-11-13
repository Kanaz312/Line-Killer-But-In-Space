using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _startHealth = 5;
    private int _health;

    // Start is called before the first frame update
    void Start() {
        _health = _startHealth;
    }


    public int Get() {
        return _health;
    }
    public void Damage(int amt) {
        _health -= amt;
        if (_health == 0) {
            Kill();
        }
    }
    public void Heal(int amt) {
        _health += amt;
    }

    public void Kill() {
        Destroy(this.gameObject);
    }
}
