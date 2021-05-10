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

    private void Update()
    {
        if(isInRange && dialogue && (Input.GetKeyDown(KeyCode.E) || forceTalk) )
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
                    if(TopDownPlayerController.Instance)
                        TopDownPlayerController.Instance.FreezeMovement();
                    else if (PlayerController.instance)
                        PlayerController.instance.Block();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            if (dialogue.isSpeaking)
            {
                dialogue.EndDialogue();                
                isInRange = false;

                if (destroyAfter)
                    Destroy(this);
            }
    }

}
