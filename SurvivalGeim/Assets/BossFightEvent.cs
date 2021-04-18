using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossFightEvent : MonoBehaviour
{

    public BoxCollider2D collider;

    public PlayableDirector director;

    public GameObject intro;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
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
                collider.enabled = false;
            };

        }
    }
}
