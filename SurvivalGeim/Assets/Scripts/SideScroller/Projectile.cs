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

    public SpriteRenderer renderer;
    public GameObject particles;
    public CircleCollider2D collider;

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
        if (isHero && collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().ReduceHealth(damage);
            StartCoroutine(WaitForSound());
        }
        else if (!isHero && collision.tag == "Player")
            StartCoroutine(WaitForSound());
    }

    private IEnumerator WaitForSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        DisableEffects();

        while (audio.isPlaying)
            yield return null;

        Destroy(gameObject);
    }

    private void DisableEffects()
    {
        collider.enabled = false;
        renderer.enabled = false;
        particles.SetActive(false);
    }

}
