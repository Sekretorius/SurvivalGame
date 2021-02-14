using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayerController : TopDownMovementController
{
    private void Update()
    {
        float verticalDirection = Input.GetAxisRaw("Vertical");
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        isRunnig = Input.GetKey(KeyCode.LeftShift);

        moveAxis = new Vector2(horizontalDirection, verticalDirection);
    }
}
