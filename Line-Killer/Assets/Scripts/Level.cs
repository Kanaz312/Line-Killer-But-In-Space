using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    private int _level = 0;
    private int _exp = 0;
    public int _upgradePoints = 0;
    private int _expNextLevel;                         

    // Start is called before the first frame update
    void Start() {
        _expNextLevel = NextLevelExp(_level+1);

    }


    // Calculate the exp required for the next level
    public static int NextLevelExp(int level) {
        float intensity = 1.4f;
        return (int)Mathf.Floor(100 * Mathf.Pow((float)level, intensity));
    }

    public int GetLevel() {
        return _level;
    }

    // Gain 1 level, 1 upgrade point, update the exp required for the next level
    public void GainLevel() {
        Debug.Log("LEVEL UP!");
        _level++;
        _upgradePoints++;
        _expNextLevel = NextLevelExp(_level+1);
    }

    public int GetExp() {
        return _exp;
    }

    // Increase the exp by the amt; check if leveled up
    public void GainExp(int amt) {
        _exp += amt;
        if (_exp >= _expNextLevel) {
            GainLevel();
        }
    }


    public int GetUpgradePoints() {
        return _upgradePoints;
    }




}
