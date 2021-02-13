using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSideScroller : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D body;

    Vector2 movement;

    private bool block = false;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}
