using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    public class BirdController : MonoBehaviour
    {
        [SerializeField]
        private Vector2 moveDirection;
        [SerializeField]
        private float speed = 0.5f;

        [SerializeField]
        private float targetDistance;

        [SerializeField]
        private SpriteRenderer spriteRenderer;
        public BirdSpawner spawnerInstance;

        public Vector2 MoveDirection { get => moveDirection; set => moveDirection = value; }
        public float TargetDistance { get => targetDistance; set => targetDistance = value; }

        private float traveledDistance = 0;

        public void FlipSprite(bool state)
        {
            spriteRenderer.flipX = state;
        }

        private void Update()
        {
            if(traveledDistance >= targetDistance)
            {
                traveledDistance = 0;
                spawnerInstance.Join(gameObject);
            }
            Vector3 moveVector = moveDirection * speed * Time.deltaTime;
            transform.position += moveVector;
            traveledDistance += Mathf.Abs(moveVector.x);
        }

        public void SetSize(Vector3 size)
        {
            transform.localScale = size;
        }
    }
}
