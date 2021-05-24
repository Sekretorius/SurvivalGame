using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkellyBossFight : MonoBehaviour
{
    public PlayableDirector director;

    public CanvasGroup canvas;

    public bool start = true;

    public Enemy boss;

    //public Speakable target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            Init();
    }

    public void Init()
    {
        CameraFollow.instance.RefreshPosition();
        if (InventoryManager.Instance)
            InventoryManager.Instance.gameObject.SetActive(false);

        EnemyManager.instance.BlockEnemyMovement();
        CameraFollow.instance.block = true;
        PlayerController.instance.Block();
        director.Play();
        //intro.transform.position = new Vector3(51,0,0);

        director.stopped += x =>
        {
            EnemyManager.instance.UnblockEnemtMovement();
            CameraFollow.instance.block = false;
            PlayerController.instance.Unblock();
        };

        canvas.alpha = 0;
    }

    public void IntroEnded()
    {
        if (InventoryManager.Instance)
            InventoryManager.Instance.gameObject.SetActive(true);

        canvas.alpha = 1;

        boss.Trigger();

        GetComponent<BoxCollider2D>().enabled = false;

        //SideScrollerPickableManager.Instance.JoinInteractableQueue(target);
        //target.isInRange = true;
        //target.forceTalk = true;
        //target.Interact();
    }
}
