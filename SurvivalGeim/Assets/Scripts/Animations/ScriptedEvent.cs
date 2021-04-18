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

    void Start()
    {
        Init();
    }

    public void Init()
    {
        animator.SetTrigger("StartIntro");
        CameraFollow.instance.block = true;
        PlayerController.instance.block = true;
        canvas.alpha = 0;
    }

    public void PlayAudio()
    {
        StartCoroutine(PlayAudioClip(delegate() 
        {
            Song.Play();
        }
        ));
    }

    private IEnumerator PlayAudioClip(Action onEnd = null)
    {
        IntroAudio.Play();

        while (IntroAudio.isPlaying) yield return null;

        onEnd?.Invoke();
    }

    private void IntroEnded()
    {
        CameraFollow.instance.block = false;
        PlayerController.instance.block = false;

        IntroAudio.volume = 0.05f;
        Song.volume = 0.05f;
        canvas.alpha = 1;
    }
}