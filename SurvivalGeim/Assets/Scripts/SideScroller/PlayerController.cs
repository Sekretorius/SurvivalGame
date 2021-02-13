using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    // Movement
    float moveVelocity;


    [SerializeField]
    public float moveSpeed = 5f;
    [SerializeField]
    public float jumpSpeed = 7.5f;
    [SerializeField]
    public float gavityScale = 2f;


    Vector2 movement;


    // Jumping
    bool jumped = false;
    public float JHeight;
    [SerializeField]
    public Rigidbody2D body;
    [SerializeField]
    public BoxCollider2D boxCollider;
    [SerializeField]
    public CompositeCollider2D groundCollider;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("IgnoreGround"));
    }

    void Update()
    {
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && !jumped)
            JumpDetected();

        if (jumped)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                moveVelocity = -moveSpeed;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                moveVelocity = moveSpeed;

            if (body.position.y < JHeight)
                resetPos();
          
            body.velocity = new Vector2(moveVelocity, body.velocity.y);

            moveVelocity = 0;
        } else // while not jumped
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
    }

    private void JumpDetected()
    {
        body.gravityScale = gavityScale;
        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        JHeight = body.position.y;
        jumped = true;
        groundCollider.gameObject.layer = LayerMask.NameToLayer("IgnoreGround");

    }

    private void resetPos()
    {
        body.MovePosition(new Vector2(body.position.x, JHeight));
        body.gravityScale = 0;
        jumped = false;
        groundCollider.gameObject.layer = LayerMask.NameToLayer("Ground");
    }

    void FixedUpdate()
    {
        if(!jumped)
            body.MovePosition(body.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}