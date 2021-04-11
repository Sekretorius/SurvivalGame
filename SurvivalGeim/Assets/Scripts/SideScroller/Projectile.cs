using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 7;
    public float maxDistance = 15;
    public int manaCost = 5;
    public float damage = 10;
    public float knockback = 1;
    public bool isHero = true;
    public bool destroyable = true;
    public bool chargeable = true;
    public bool rotatable = true;
    public KeyCode keyCode;


    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = transform.right * projectileSpeed;
        Destroy(gameObject, maxDistance / projectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(destroyable) Destroy(gameObject);
    }

}
