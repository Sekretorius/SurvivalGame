using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickType { Interaction, Collision }
public class PickableItem : MonoBehaviour
{
    [SerializeField]
    protected PickType pickType = PickType.Collision;
    [SerializeField]
    protected Collider2D itemCollider;

    public bool IsPicked { get; set; }

    protected virtual void Awake()
    {
       if(itemCollider == null)
        {
            itemCollider = GetComponent<Collider2D>();
        }
    }
    protected virtual void Start()
    {

    }

    public virtual void Interact()
    {

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        
    }
    protected virtual void OnCollisionExit2D(Collision2D collision)
    {

    }
}
