using InteractionSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Speakable : Interactable
{
    [SerializeField]
    public Dialogue dialogue;

    public bool isInRange = false;
    public bool forceTalk = false;
    public bool destroyAfter = false;
    public bool playerMovement = true;

    public UnityEvent onEnd;

    public override void Interact()
    {
        if ( dialogue && isInRange)
        {
            if (dialogue.isSpeaking)
            {
                dialogue.Next();
            }
            else
            {
                DialogueController.instance.Enable();
                dialogue.StartDialogue(destroyAfter, onEnd);

                if (!playerMovement)
                {
                    if (TopDownPlayerController.Instance)
                        TopDownPlayerController.Instance.FreezeMovement();
                    else if (PlayerController.instance)
                        PlayerController.instance.Block();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInRange = false;
        OnInteractionRangeExit();
    }

    public override void OnInteractionRangeExit()
    {
        if (dialogue.isSpeaking)
        {
            dialogue.EndDialogue();
        }
    }

    public void Destroy()
    {
        if (destroyAfter)
            Destroy(this);
    }
}
