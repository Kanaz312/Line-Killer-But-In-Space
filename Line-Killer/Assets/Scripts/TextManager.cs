using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{

    private Player _player;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI exp;

    // Start is called before the first frame update
    void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        hp.text = "Health: " + _player.GetComponent<Health>().Get();
        exp.text = "Exp: " + _player.GetComponent<Level>().GetExp() + " / Level " + _player.GetComponent<Level>().GetLevel();


    }
}
