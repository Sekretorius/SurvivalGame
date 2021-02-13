using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite released;
    [SerializeField]
    private Sprite pressed;

    public bool pressedBtn = false;
    private bool current = false;

    void Start()
    {
        image.sprite = released;
    }

    private void Update()
    {
        if (pressedBtn)
            Press();
        else
            Release();
    }

    public void Press()
    {
        if (!current)
        {
            image.sprite = pressed;
            current = true;
        }
    }

    public void Release()
    {
       if (current)
       {
           image.sprite = released;
           current = false;
       }
    }
}
