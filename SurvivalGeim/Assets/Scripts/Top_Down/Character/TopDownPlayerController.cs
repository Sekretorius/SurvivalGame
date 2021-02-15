using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayerController : TopDownMovementController
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public static TopDownPlayerController Instance;

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
    }
}
