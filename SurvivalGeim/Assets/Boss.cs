using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Enter exit;

    private void OnDisable()
    {
        exit.canExit = true;
    }
}
