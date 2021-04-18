using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemies : MonoBehaviour
{
    public void Trigger()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Enemy>().Trigger();
            Debug.Log("Triggered");
        }
    }
}
