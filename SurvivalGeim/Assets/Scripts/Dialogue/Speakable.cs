using InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speakable : Interactable
{
    [SerializeField]
    public Dialogue dialogue;

    public bool isInRange = false;
    public bool forceTalk = false;
    public bool destroyAfter = false;
    public bool playerMovement = true;
    public override void Interact()
    {
        if (dialogue || forceTalk)
        {
            if (dialogue.isSpeaking)
                dialogue.Next();
            else
            {
                forceTalk = false;

                DialogueController.instance.Enable();
                dialogue.StartDialogue();

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
    public override void OnInteractionRangeExit()
    {
        if (dialogue.isSpeaking)
        {
            dialogue.EndDialogue();
            if (destroyAfter)
                Destroy(this);
        }
    }
}
