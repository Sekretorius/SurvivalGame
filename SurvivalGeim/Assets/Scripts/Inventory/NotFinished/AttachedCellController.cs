using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class AttachedCellController : MonoBehaviour
{
    private Image cellImage;
    private List<Collision2D> contacts = new List<Collision2D>();
    private BoxCollider2D boxCollider;

    private Collision2D reservedCell = null;
    public class CellReleaseEvent : UnityEvent <Collision2D>{ };
    public static CellReleaseEvent CellReleaseEventHandler;

    private void Awake()
    {
        if(CellReleaseEventHandler == null)
        {
            CellReleaseEventHandler = new CellReleaseEvent();
        }
    }
    private void Start()
    {
        cellImage = GetComponent<Image>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = cellImage.rectTransform.rect.size;
    }
    private void HandleCellReleaseEvent(Collision2D collision2D)
    {
        if (contacts.Contains(collision2D))
        {
            if (InventoryItemArea.Instance.ReserveCell(collision2D.collider))
            {
                if (cellImage != null)
                {
                    cellImage.color = Color.green;
                }
                reservedCell = collision2D;
                CellReleaseEventHandler.RemoveListener(HandleCellReleaseEvent);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        contacts.Add(collision);
        if (InventoryItemArea.Instance.ReserveCell(collision.collider))
        {
            CellReleaseEventHandler.RemoveListener(HandleCellReleaseEvent);
            if (reservedCell != null)
            {
                InventoryItemArea.Instance.ReleaseCell(reservedCell.collider);
                CellReleaseEventHandler.Invoke(reservedCell);
            }
            reservedCell = collision;
            if (cellImage != null)
            {
                cellImage.color = Color.green;
            }
        }
        if(reservedCell == null)
        {
            CellReleaseEventHandler.RemoveListener(HandleCellReleaseEvent);
            CellReleaseEventHandler.AddListener(HandleCellReleaseEvent);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (contacts.Contains(collision))
        {
            if (reservedCell == collision)
            {
                InventoryItemArea.Instance.ReleaseCell(collision.collider);
                CellReleaseEventHandler.Invoke(collision);
                reservedCell = null;
            }
            contacts.Remove(collision);
            if (cellImage != null && contacts.Count == 0)
            {
                cellImage.color = Color.red;
            }
        }
    }
}
