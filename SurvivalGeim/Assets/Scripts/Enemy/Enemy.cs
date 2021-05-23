using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2 enemyPos;

    public float movementSpeed = 0.05f;

    Vector2 movement;
    Vector2 target;

    float offset;

    bool triggered = false;

    bool knockBack = false;

    public bool block = false;

    bool attack = true;

    [SerializeField]
    [Range(0f,10f)]
    public float attackRange = 2;

    [SerializeField]
    [Range(0f, 10f)]
    public float triggerRange = 5;


    [SerializeField]
    public int damage = 10;

    [SerializeField]
    public float attackSpeed;

    [SerializeField]
    public float maxHealth = 100;

    [SerializeField]
    public float health = 100;

    private float hScale = 1;

    [SerializeField]
    public SpriteRenderer healthBar;

    [SerializeField]
    public BoxCollider2D boxCollider;

    [SerializeField]
    public Rigidbody2D body;

    [SerializeField]
    public SpriteRenderer sprite;

    public Animator animator;

    public ParticleSystem particles;

    [SerializeField]
    private EntityDropManager entityDropManager;

    float JHeight = 0;
    bool knocked = false;

    public void Awake()
    {
        if (boxCollider == null)
            boxCollider = gameObject.GetComponent<BoxCollider2D>();

        if (body == null)
            body = gameObject.GetComponent<Rigidbody2D>();

        if(entityDropManager == null)
            entityDropManager = gameObject.GetComponent<EntityDropManager>();

    }

    // Start is called before the first frame update
    void Start()
    {
        offset = boxCollider.size.x / 2;
        health = maxHealth;
        hScale = healthBar.transform.localScale.x;
        Physics2D.IgnoreCollision(boxCollider, PlayerController.instance.boxCollider);
        Debug.Log("Collisions ignored");
    }


    // Update is called once per frame
    void Update()
    {
        enemyPos = boxCollider.bounds.center;
        movement = Vector2.MoveTowards(enemyPos, PlayerController.instance.playerPos, movementSpeed);

        CheckIfTrigger();

        if (Vector2.Distance(enemyPos, PlayerController.instance.playerPos) <= attackRange && attack)
            StartCoroutine(AttackStart());

        if (knocked && body.position.y < JHeight)
            resetPos();

        
    }

    void FixedUpdate()
    {
        if (triggered && Distance() > offset && !block && !knocked)
        {
            body.MovePosition(movement);
        }

        if ((enemyPos - PlayerController.instance.playerPos).normalized.x > 0)
            sprite.flipX = true;
        else
            sprite.flipX = false;
    }

    public void Knockback(float direction = 0, float strenght = 1)
    {
        body.gravityScale = strenght;

        body.velocity = new Vector2(direction, 1);
        JHeight = body.position.y;

        knocked = true;
    }

    private void resetPos()
    {
        body.MovePosition(new Vector2(body.position.x, JHeight));
        body.gravityScale = 0;
        knocked = false;
    }

    public void ReduceHealth(float damageTaken)
    {
        triggered = true;
        animator.SetBool("Triggered", triggered);

        health -= damageTaken;

        if (health <= 0)
        {
            entityDropManager?.Drop();
            Destroy(gameObject);
        }

        StartCoroutine(Injured());

        Vector3 scale = healthBar.transform.localScale;
        healthBar.transform.localScale = new Vector3((health / maxHealth) * hScale, scale.y , scale.z);

        particles.Play();
    }

    public void CheckIfTrigger()
    {
        if (Vector2.Distance(enemyPos, PlayerController.instance.playerPos) <= triggerRange && triggered == false)
            Trigger();
    }

    public void Trigger()
    {
        triggered = true;
        animator.SetBool("Triggered", triggered);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Punch") {
            ReduceHealth(PlayerController.instance.punchStrengh);
            return;
        }

        if (collision.tag == "Projectile")
        {
            Debug.Log("Knocked");
            //Knockback(collision.transform.position.x, collision.GetComponent<Projectile>().knockback);
            Knockback(collision.transform.position.x);
        }

        // Knockback
       // if (triggered && collision.tag == "Player" && !knockBack)
            //StartCoroutine(Timer(Knockback));
    }

    IEnumerator Injured()
    {
        if(sprite.color == Color.red)
            yield break;

        Color old = sprite.color;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        sprite.color = old;
    }

    IEnumerator Timer(Action action)
    {
        action.Invoke();
        knockBack = true;

        yield return new WaitForSeconds(0.5f);

        CameraFollow.instance.block = false;

        yield return new WaitForSeconds(3);
        knockBack = false;
    }

    IEnumerator AttackStart()
    {
        attack = false;
        block = true;
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(attackSpeed);
        attack = true;
        block = false;
    }

    private void Knockback()
    {
        //Vector2 diff = PlayerController.instance.transform.position - transform.position;
        PlayerController.instance.GetKnocked(PlayerController.instance.transform.position - transform.position, 2);
        Debug.Log("Player got knocked!");
    }

    public void Attack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(enemyPos, attackRange);
        for (int i = 0; i < enemiesToDamage.Length; i++)
            if (enemiesToDamage[i].tag == "Player")
            {
                StartCoroutine(Timer(Knockback));
                PlayerManager.instance.ChangeHealth(-damage);
                break;
            }
    }


    public float Distance()
    {
        return Vector2.Distance(enemyPos, PlayerController.instance.playerPos);
    }


    private void OnDrawGizmos()
    {
        if(PlayerController.instance != null)
        {
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(enemyPos, PlayerController.instance.playerPos);
            Gizmos.DrawWireSphere(enemyPos, triggerRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(enemyPos, attackRange);
        }
    }

}
