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
    public Sprite barSprite;

    public float damageRate = 0.5f;
    

    public SpriteRenderer renderer;
    public GameObject particles;
    public CircleCollider2D collider;

    private float damageRateElapsedTime = 0;
    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = transform.right * projectileSpeed;
        Destroy(gameObject, maxDistance / projectileSpeed);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isHero && collision.tag == "Enemy")
        {

            if (damageRateElapsedTime <= 0)
            {
                collision.GetComponent<Enemy>().ReduceHealth(damage);
                StartCoroutine(WaitForSound());
                damageRateElapsedTime = damageRate;
            }
            else
            {
                damageRateElapsedTime -= Time.deltaTime;
            }
        }
        else if (!isHero && collision.tag == "Player")
            StartCoroutine(WaitForSound());
    }

    private IEnumerator WaitForSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (destroyable) DisableEffects();

        while (audio.isPlaying)
            yield return null;

        if(destroyable) Destroy(gameObject);
    }

    private void DisableEffects()
    {
        collider.enabled = false;
        renderer.enabled = false;
        particles.SetActive(false);
    }

}
