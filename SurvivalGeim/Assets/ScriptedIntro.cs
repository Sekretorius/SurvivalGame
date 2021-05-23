using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ScriptedIntro : MonoBehaviour
{
    public PlayableDirector director;

    public CanvasGroup canvas;

    public bool start = true;

    public Speakable target;

    void Start()
    {
        if (start)
            Init();
    }

    public void Init()
    {
        CameraFollow.instance.RefreshPosition();
        if (InventoryManager.Instance)
            InventoryManager.Instance.gameObject.SetActive(false);

        CameraFollow.instance.block = true;
        PlayerController.instance.block = true;

        director.Play();

        canvas.alpha = 0;
    }

    public void IntroEnded()
    {
        CameraFollow.instance.block = false;
        PlayerController.instance.block = false;
        if (InventoryManager.Instance)
            InventoryManager.Instance.gameObject.SetActive(true);

        canvas.alpha = 1;

        SideScrollerPickableManager.Instance.JoinInteractableQueue(target);
        target.isInRange = true;
        target.forceTalk = true;
        target.Interact();
    }
}
