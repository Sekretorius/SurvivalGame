using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputs : MonoBehaviour
{
    [SerializeField]
    private List<Key> keys;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            keys[0].pressedBtn = true;
        else
            keys[0].pressedBtn = false;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            keys[1].pressedBtn = true;
        else
            keys[1].pressedBtn = false;

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            keys[2].pressedBtn = true;
        else
            keys[2].pressedBtn = false;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            keys[3].pressedBtn = true;
        else
            keys[3].pressedBtn = false;

        if (Input.GetKey(KeyCode.Space))
            keys[4].pressedBtn = true;
        else
            keys[4].pressedBtn = false;
    }
}
