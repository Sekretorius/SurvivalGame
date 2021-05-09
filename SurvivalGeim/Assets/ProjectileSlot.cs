using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileSlot : MonoBehaviour
{
    // Start is called before the first frame update
    public KeyCode keyCode;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            gameObject.transform.Find("Background").Find("Cover").gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(keyCode))
        {
            gameObject.transform.Find("Background").Find("Cover").gameObject.SetActive(false);
        }
    }
}
