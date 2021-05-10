using InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speakable : Interactable
{
    [SerializeField]
    public Dialogue dialogue;

    public bool isInRange = false;

    private void Update()
    {
        if(isInRange && dialogue && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogue.isSpeaking)
                dialogue.Next();
            else
            {
                DialogueController.instance.Enable();
                dialogue.StartDialogue();
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
            }
    }

}
