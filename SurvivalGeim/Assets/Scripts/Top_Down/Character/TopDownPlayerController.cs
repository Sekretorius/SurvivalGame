using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InteractionSystem;
public class TopDownPlayerController : TopDownMovementController
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Image interactionButton;
    public static TopDownPlayerController Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        interactionButton?.gameObject.SetActive(false);
    }
    public void UpdateSpriteLayer(int layer)
    {
        spriteRenderer.sortingOrder = layer;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        float verticalDirection = Input.GetAxisRaw("Vertical");
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        isRunnig = Input.GetKey(KeyCode.LeftShift);

        moveAxis = new Vector2(horizontalDirection, verticalDirection);

        Animate();

    }

    private void Animate()
    {
        animator.SetBool("IsRunning", isRunnig);
        if (moveAxis == Vector2.zero || isMovementFreezed)
        {
            animator.SetInteger("WalkingDirection", 0);
            animator.SetBool("IsWalking", false);
        }
        else
        {
            animator.SetBool("IsWalking", true);
            if (moveAxis.x != 0)
            {
                animator.SetInteger("WalkingDirection", 2);
                if (moveAxis.x > 0)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }
            else
            {
                spriteRenderer.flipX = false;
            }
            if (moveAxis.y > 0)
            {
                animator.SetInteger("WalkingDirection", 1);
            }
            else if (moveAxis.y < 0)
            {
                animator.SetInteger("WalkingDirection", -1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            //to do: create interface with interactable;
            Interactable interactable = collision.GetComponent<Interactable>();
            if(interactable != null)
            {
                JoinInteractableQueue(interactable);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            Interactable interactable = collision.GetComponent<Interactable>();
            if (interactable != null)
            {
                RemoveInteractableFromQueue(interactable);
            }
        }
    }

    private LinkedList<Interactable> interactables = new LinkedList<Interactable>();

    private void Interact()
    {
        if(interactables.Count > 0)
        {
            Interactable interactable = interactables.First.Value;
            RemoveInteractableFromQueue(interactable);
            interactable.Interact();
        }
    }
    protected override void UpdateOnMove()
    {
        UpdateInteractableDistances();
    }

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
            interactionButton?.gameObject.SetActive(true);
        }
        else
        {
            interactionButton?.gameObject.SetActive(false);
        }
    }
    public void RemoveInteractableFromQueue(Interactable interactable)
    {
        if (interactables.Contains(interactable))
        {
            Interactable firstItem = interactables.First.Value;
            interactables.Remove(interactable);
            if (firstItem == interactable)
            {
                UpdateInteractableDistances();
            }
        }
    }


}
