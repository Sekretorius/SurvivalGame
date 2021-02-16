using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    void Start()
    {
        if (EnterManager.instance.playerPos != Vector3.zero)
            gameObject.transform.position = EnterManager.instance.playerPos;
    }

}
