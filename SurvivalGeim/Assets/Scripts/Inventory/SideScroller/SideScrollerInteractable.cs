using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteractionSystem;

public class SideScrollerInteractable : MonoBehaviour
{
    [SerializeField]
    private InventoryPickableItem inventoryPickable;

    private void Awake()
    {
        if(inventoryPickable == null)
        {
            inventoryPickable = gameObject.GetComponent<InventoryPickableItem>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SideScrollerPickableManager.Instance.JoinInteractableQueue(inventoryPickable);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SideScrollerPickableManager.Instance.RemoveInteractableFromQueue(inventoryPickable);
        }
    }
}
