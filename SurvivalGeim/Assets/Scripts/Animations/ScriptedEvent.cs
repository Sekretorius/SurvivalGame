using InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvent : MonoBehaviour
{

    public AudioSource IntroAudio;

    public AudioSource Song;

    public Animator animator;

    public CanvasGroup canvas;

    public Speakable target;

    public bool start = true;

    void Start()
    {
        if(start)
            Init();
    }

    public void Init()
    {
        CameraFollow.instance.RefreshPosition();
        if (InventoryManager.Instance)
            InventoryManager.Instance.gameObject.SetActive(false);
        animator.SetTrigger("StartIntro");
        CameraFollow.instance.block = true;
        PlayerController.instance.block = true;

        canvas.alpha = 0;
    }

    public void PlayAudio()
    {
        StartCoroutine(PlayAudioClip(delegate() 
        {
           // Song.Play();
        }
        ));
    }

    private IEnumerator PlayAudioClip(Action onEnd = null)
    {
        IntroAudio.time = 44.5f;
        IntroAudio.Play();

        while (IntroAudio.isPlaying) yield return null;

        onEnd?.Invoke();
    }

    private void IntroEnded()
    {
        CameraFollow.instance.block = false;
        PlayerController.instance.block = false;
        if (InventoryManager.Instance)
            InventoryManager.Instance.gameObject.SetActive(true);

        IntroAudio.volume = 0.05f;
        Song.volume = 0.05f;
        canvas.alpha = 1;

        target.isInRange = true;
        target.forceTalk = true;
    }
}
