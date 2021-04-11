using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            InventoryPickableItem item = collision.GetComponent<InventoryPickableItem>();
            if(item != null)
            {
                JoinPickableItemQueue(item);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            InventoryPickableItem item = collision.GetComponent<InventoryPickableItem>();
            if (item != null)
            {
                RemovePickableItemFromQueue(item);
            }
        }
    }

    private LinkedList<InventoryPickableItem> interactableItems = new LinkedList<InventoryPickableItem>();

    private void Interact()
    {
        if(interactableItems.Count > 0)
        {
            InventoryPickableItem item = interactableItems.First.Value;
            RemovePickableItemFromQueue(item);
            item.Interact();
        }
    }
    protected override void UpdateOnMove()
    {
        UpdatePickableItemDistances();
    }

    public void JoinPickableItemQueue(InventoryPickableItem item)
    {
        interactableItems.AddLast(item);
        UpdatePickableItemDistances();
    }

    public void UpdatePickableItemDistances()
    {
        if (interactableItems.Count > 0)
        {
            LinkedListNode<InventoryPickableItem> first = interactableItems.First;
            LinkedListNode<InventoryPickableItem> newFirst = first;
            Vector2 playerPosition = transform.position;
            float shortestDistance = Vector2.Distance(playerPosition, first.Value.transform.position);

            for (LinkedListNode<InventoryPickableItem> current = interactableItems.First; current != null; current = current.Next)
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
                interactableItems.Remove(newFirst);
                interactableItems.AddFirst(newFirst);
            }
            interactionButton?.gameObject.SetActive(true);
        }
        else
        {
            interactionButton?.gameObject.SetActive(false);
        }
    }
    public void RemovePickableItemFromQueue(InventoryPickableItem item)
    {
        if (interactableItems.Contains(item))
        {
            InventoryPickableItem firstItem = interactableItems.First.Value;
            interactableItems.Remove(item);
            if (firstItem == item)
            {
                UpdatePickableItemDistances();
            }
        }
    }


}
