using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public abstract class TileBehavior : MonoBehaviour
{
    [Serializable]
    public class OnTileTriggerEvent : UnityEvent<Collider2D, bool> { }
    [SerializeField]
    protected OnTileTriggerEvent OnTileSteppedEvent = new OnTileTriggerEvent();

    [SerializeField]
    private bool playerCanActivate = true;

    public bool IsSteppedByPlayer { get; set; } = false;
    protected virtual void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventCaller(collision, true);
        if (!playerCanActivate)
        {
            TileAllEffector(collision, true);
            return;
        }
        if (collision.TryGetComponent(out CharacterEventHandler controller))
        {
            IsSteppedByPlayer = true;
            TileEffector(controller, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EventCaller(collision, false);
        if (!playerCanActivate)
        {
            TileAllEffector(collision, false);
            return;
        }
        if (collision.TryGetComponent(out CharacterEventHandler controller))
        {
            IsSteppedByPlayer = false;
            TileEffector(controller, false);
        }
    }
    protected virtual void TileEffector(CharacterEventHandler controller, bool state)
    {
    }
    protected virtual void TileAllEffector(Collider2D collider, bool state)
    {
    }
    protected virtual void EventCaller(Collider2D collision, bool state)
    {
        OnTileSteppedEvent.Invoke(collision, state);
    }
}
