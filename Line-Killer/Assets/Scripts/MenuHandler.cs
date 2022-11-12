using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{

    KeyCode keyQuit = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {

        // Quit game
        if (Input.GetKeyDown(keyQuit)) {
            Application.Quit();
        }

    }

}

