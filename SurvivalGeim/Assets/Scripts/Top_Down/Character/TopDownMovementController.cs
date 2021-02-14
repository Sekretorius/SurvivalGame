using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterEventHandler))]
public class TopDownMovementController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float runSpeed = 3f;

    private const float colliderCheckOffset = .01f;
    private const float colliderForce = 0.5f;
    private BoxCollider2D boxCollider;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
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

    private void FixedUpdate()
    {
        Move(new Vector2(moveAxis.x, 0), currentSpeed * Time.fixedDeltaTime + colliderCheckOffset);
        Move(new Vector2(0, moveAxis.y), currentSpeed * Time.fixedDeltaTime + colliderCheckOffset);

        moveAxis = Vector2.zero;
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

                moveDirection = -raycastHit2D.normal * (projectionLength - colliderCheckOffset); 
                Vector2.ClampMagnitude(-raycastHit2D.normal * (projectionLength - colliderCheckOffset), distance);
            }
        }
        transform.position += (Vector3)moveDirection;
    }
    private RaycastHit2D GetPlayerHit(Collider2D collider2D, Vector2 direction, float distance)
    {
        RaycastHit2D[] results = new RaycastHit2D[10];
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        collider2D.Cast(direction, contactFilter2D, results, distance);
        foreach (RaycastHit2D hit in results)
        {
            if (hit)
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
        boxCollider.Cast(direction, contactFilter2D, results, distance);
        foreach(RaycastHit2D hit in results)
        {
            if(hit)
            {
                return hit;
            }
        }

        return new RaycastHit2D();
    }
}
