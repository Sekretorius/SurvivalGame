using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayerController : TopDownMovementController
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;

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
    }
    public void UpdateSpriteLayer(int layer)
    {
        spriteRenderer.sortingOrder = layer;
    }
    private void Update()
    {
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
}
