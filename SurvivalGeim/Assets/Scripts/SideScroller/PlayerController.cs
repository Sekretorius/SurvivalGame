using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    // Movement
    float moveVelocity;

    public bool block = false;

    [SerializeField]
    public float moveSpeed = 5f;
    [SerializeField]
    public float jumpSpeed = 7.5f;
    [SerializeField]
    public float gavityScale = 2f;

    Vector2 movement;

    // Jumping
    bool jumped = false;
    bool control = true;
    public float JHeight;
    [SerializeField]
    public Rigidbody2D body;
    [SerializeField]
    public BoxCollider2D boxCollider;
    [SerializeField]
    public CompositeCollider2D groundCollider;
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public SpriteRenderer sprite;

    public Vector2 playerPos;

    [SerializeField]
    public GameObject punches;

    public float punchStrengh = 2;

    bool idle = true;

    public ParticleSystem particles;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

    }

    private void Start()
    {
        playerPos = boxCollider.bounds.center;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("IgnoreGround"));
        animator.SetBool("Idle", true);
    }

    void Update()
    {
        if (block)
            return;
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && !jumped)
            JumpDetected();

        if (jumped && control)
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

        if(Input.GetKeyDown(KeyCode.Mouse0))
            animator.SetBool("Attack", true);

        playerPos = boxCollider.bounds.center;

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(playerPos, new Vector3(0.2f, 0.2f, 0.2f));
    }


    private void JumpDetected(float direction = 0)
    {
        body.gravityScale = gavityScale;
        if (direction == 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            JHeight = body.position.y;
        }
        else
            body.velocity = new Vector2(direction, jumpSpeed);
        
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

    public void OnAttackEnded()
    {
        animator.SetBool("Attack", false);
    }

    void FixedUpdate()
    {
        if (block)
            return;

        Animate();

        if (!jumped)
            body.MovePosition(body.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void Animate()
    {
        if (!idle && movement == Vector2.zero)
        {
            idle = true;
            animator.SetBool("Idle", true);
        }
        else if (idle && movement != Vector2.zero)
        {
            idle = false;
            animator.SetBool("Idle", false);
        }

        if (movement.x > 0 && sprite.flipX)
        {
            SwapArms(0);
            sprite.flipX = false;
        }
        else if (movement.x < 0 && !sprite.flipX)
        {
            SwapArms(180);
            sprite.flipX = true;
        }  
    }

    private void SwapArms(float rotate)
    {
        Quaternion q = punches.transform.localRotation;
        punches.transform.localRotation = new Quaternion(q.x, rotate, q.z,q.w);
        Vector3 p = punches.transform.localPosition;
        Debug.Log(p);
        punches.transform.localPosition = new Vector3(-p.x,p.y,p.z);
    }

    public void Block()
    {
        if (this != null)
        {
            block = true;
            body.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    public void Unblock()
    {
        if (this != null) 
        { 
            block = false;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
	}

    public void GetKnocked(Vector2 position, float power)
    {
        StartCoroutine(WaitForFixed(delegate()
        {
            //CameraFollow.instance.block = true;
            control = false;
            JumpDetected(position.x * power);
        }));    
    }


    IEnumerator WaitForFixed(Action action)
    {
        yield return new WaitForFixedUpdate();
        action.Invoke();

        yield return new WaitForSeconds(0.5f);
        control = true;
    }
}