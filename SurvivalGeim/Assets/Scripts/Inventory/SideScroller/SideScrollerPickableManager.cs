using InteractionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollerPickableManager : MonoBehaviour
{
    public static SideScrollerPickableManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    private void FixedUpdate()
    {
        UpdateInteractableDistances();
    }

    private LinkedList<Interactable> interactables = new LinkedList<Interactable>();

    private void Interact()
    {
        if (interactables.Count > 0)
        {
            Interactable interactable = interactables.First.Value;
            //RemoveInteractableFromQueue(interactable);
            interactable.Interact();
        }
    }
    //protected override void UpdateOnMove()
    //{
    //    UpdateInteractableDistances();
    //}

    public void JoinInteractableQueue(Interactable interactable)
    {
        interactables.AddLast(interactable);
        UpdateInteractableDistances();
    }

    public void UpdateInteractableDistances()
    {
        if (interactables.Count > 0)
        {
            LinkedListNode<Interactable> first = interactables.First;
            LinkedListNode<Interactable> newFirst = first;
            Vector2 playerPosition = transform.position;
            float shortestDistance = Vector2.Distance(playerPosition, first.Value.transform.position);

            for (LinkedListNode<Interactable> current = interactables.First; current != null; current = current.Next)
            {
                float nodeDistance = Vector2.Distance(playerPosition, current.Value.transform.position);
                if (nodeDistance < shortestDistance)
                {
                    shortestDistance = nodeDistance;
                    newFirst = current;
                }
            }
            if (first != newFirst)
            {
                interactables.Remove(newFirst);
                interactables.AddFirst(newFirst);
            }
            //interactionButton?.gameObject.SetActive(true);
        }
        else
        {
            //interactionButton?.gameObject.SetActive(false);
        }
    }
    public void RemoveInteractableFromQueue(Interactable interactable)
    {
        if (interactables.Contains(interactable))
        {
            Interactable firstItem = interactables.First.Value;
            interactables.Remove(interactable);
            interactable.OnInteractionRangeExit();
            if (firstItem == interactable)
            {
                UpdateInteractableDistances();
            }
        }
    }
}
