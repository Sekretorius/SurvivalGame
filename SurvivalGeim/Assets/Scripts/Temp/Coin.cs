using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AnimationSystem;
using InteractionSystem;

public class Coin : Interactable
{
    [SerializeField]
    private int minValue = 1;
    [SerializeField]
    private int maxValue = 50;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private RectTransform rect;

    [SerializeField]
    private FadeAnimation fadeAnimation;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;
    protected override void Start()
    {
        base.Start();
        fadeAnimation.SpriteRenderer = spriteRenderer;
        fadeAnimation.Init((IEnumerator enumerator) => { StartCoroutine(enumerator); });

        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInteracted)
        {
            return;
        }
        if (collision.collider.name.Equals("Player"))
        {
            audioSource.PlayOneShot(audioClip);
            int value = Random.Range(minValue, maxValue);
            PlayerManager.instance.ChangeMoney(value);
            IsInteracted = true;
            fadeAnimation.OnAnimationEnd.AddListener(() => { StartCoroutine(DestroyAtSoundFinish()); });
            fadeAnimation.StartAnimation();
        }
    }

    private IEnumerator DestroyAtSoundFinish()
    {
        while (audioSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}