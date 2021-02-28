using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class AttachedCellController : MonoBehaviour
{
    public Image cellImage { get; set; }
    private List<Collision2D> contacts = new List<Collision2D>();
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = cellImage.rectTransform.sizeDelta;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        contacts.Add(collision);
        if (cellImage != null)
        {
            cellImage.color = Color.green;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        contacts.Remove(collision);
        if (cellImage != null && contacts.Count == 0)
        {
            cellImage.color = Color.red;
        }
    }
}
