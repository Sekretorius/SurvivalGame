using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private AudioSource audio;

    public void OnTriggerAttack()
    {
        enemy.Attack();
        enemy.animator.SetBool("Attack", false);
    }

    public void PlayAttackSound()
    {
        audio.Play();
    }

}
