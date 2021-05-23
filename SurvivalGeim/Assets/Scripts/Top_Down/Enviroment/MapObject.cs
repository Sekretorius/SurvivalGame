using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private Vector2 range = new Vector2(0f, 1f);

    private void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.SetFloat("Offset", Random.Range(range.x, range.y));
    }
}
