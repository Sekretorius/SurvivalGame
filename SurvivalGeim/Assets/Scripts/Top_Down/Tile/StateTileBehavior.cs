using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateTileBehavior : TileBehavior
{
    [SerializeField]
    private List<GameObject> effectedObjects = new List<GameObject>();
    [SerializeField]
    private bool rotateState = false;
    [SerializeField]
    private bool setStateTo = true;
 
    protected override void Start()
    {
        base.Start();
        if (!rotateState)
        {
            foreach (GameObject gameObject in effectedObjects)
            {
                gameObject.SetActive(!setStateTo);
            }
        }
    }
    protected override void TileEffector(CharacterEventHandler controller, bool state)
    {
        SetState();
    }
    protected override void TileAllEffector(Collider2D collider, bool state)
    {
    }

    private void SetState()
    {
        if (!rotateState && setStateTo == gameObject.activeInHierarchy)
        {
            return;
        }
        foreach (GameObject gameObject in effectedObjects)
        {
            if (rotateState)
            {
                gameObject.SetActive(!gameObject.activeInHierarchy);
            }
            else
            {
                gameObject.SetActive(setStateTo);
            }
        }
    }
}
