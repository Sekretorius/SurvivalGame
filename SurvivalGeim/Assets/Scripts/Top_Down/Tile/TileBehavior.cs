using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CharacterEventHandler controller))
        {
            TileEffector(controller);
        }
    }
    protected virtual void TileEffector(CharacterEventHandler controller)
    {
        Debug.Log(controller.name);
    }
}
