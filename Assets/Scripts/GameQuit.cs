using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuit : MonoBehaviour {

    void Quit()
    {
        Application.Quit();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Quit();
    }
}
