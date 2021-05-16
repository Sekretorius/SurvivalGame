using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterEventHandler))]
public class TopDownMovementController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float runSpeed = 3f;

    private const float colliderCheckOffset = .01f;
    private Collider2D objectCollider;
    private float currentSpeed
    {
        get
        {
            if (isRunnig)
                return runSpeed;
            else
                return moveSpeed;
        }
    }


    protected Vector2 moveAxis = Vector2.zero;
    protected bool isRunnig = false;
    protected bool isMovementFreezed = false;

    private void Start()
    {
        objectCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (!isMovementFreezed && Vector2.SqrMagnitude(moveAxis) > 0)
        {
            Move(new Vector2(moveAxis.x, 0), currentSpeed * Time.fixedDeltaTime + colliderCheckOffset);
            Move(new Vector2(0, moveAxis.y), currentSpeed * Time.fixedDeltaTime + colliderCheckOffset);
        }

        moveAxis = Vector2.zero;
    }

    /// <summary>
    /// Freezes player movement and sets moving animation to idle
    /// </summary>
    public void FreezeMovement()
    {
        isMovementFreezed = true;
    }
    /// <summary>
    /// Unfreezes player movement and animations
    /// </summary>
    public void UnFreezeMovement()
    {
        isMovementFreezed = false;
    }

    private void Move(Vector2 direction, float distance)
    {
        RaycastHit2D raycastHit2D = OnCollision(direction, distance);
        Vector2 moveDirection = Vector2.zero;
        if (!raycastHit2D)
        {
            moveDirection = direction * currentSpeed * Time.deltaTime;
        }
        else
        {
            RaycastHit2D playerHit = GetPlayerHit(raycastHit2D.collider, raycastHit2D.normal, distance);
            if (playerHit)
            {
                float projectionLength = Vector2.Dot(raycastHit2D.normal, playerHit.point - raycastHit2D.point) / (Vector2.SqrMagnitude(raycastHit2D.normal));

                moveDirection = Vector2.ClampMagnitude(-raycastHit2D.normal * (projectionLength - colliderCheckOffset), distance);//-raycastHit2D.normal * (projectionLength - colliderCheckOffset);
            }
        }
        if(moveDirection != Vector2.zero)
        {
            UpdateOnMove();
        }
        transform.position += (Vector3)moveDirection;
    }
    protected virtual void UpdateOnMove()
    {

    }
    private RaycastHit2D GetPlayerHit(Collider2D collider2D, Vector2 direction, float distance)
    {
        RaycastHit2D[] results = new RaycastHit2D[10];
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        collider2D.Cast(direction, contactFilter2D, results, distance);
        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider == objectCollider)
            {
                return hit;
            }
        }
        return new RaycastHit2D();
    }
    private RaycastHit2D OnCollision(Vector2 direction, float distance)
    {
        RaycastHit2D[] results = new RaycastHit2D[10];

        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.layerMask = ~LayerMask.GetMask("Walkable");
        contactFilter2D.useLayerMask = true;

        objectCollider.Cast(direction, contactFilter2D, results, distance);
        foreach(RaycastHit2D hit in results)
        {
            if (hit)
            {
                return hit;
            }
        }

        return new RaycastHit2D();
    }
}
