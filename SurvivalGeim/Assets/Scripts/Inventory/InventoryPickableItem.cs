using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using UnityEngine.UI;
using AnimationSystem;
using System;

namespace InteractionSystem
{
    public class InventoryPickableItem : Interactable
    {
        [SerializeField]
        private InventoryItem inventoryItemData;
        [SerializeField]
        private SpriteRenderer itemSprite;
        [SerializeField]
        private int itemAmount;
        [SerializeField]
        private FadeAnimation fadeAnimation;

        private TransformAnimation transformAnimation; 
        public int ItemAmount => itemAmount;
        public InventoryItem InventoryItem => inventoryItemData;

        protected override void Start()
        {
            base.Start();


            fadeAnimation.SpriteRenderer = itemSprite;

            fadeAnimation.Init((IEnumerator enumerator) => { StartCoroutine(enumerator); });
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsInteracted) return;
            if (!inventoryItemData.Interactable && !collision.collider.isTrigger && collision.collider.name.Equals("Player"))
            {
                AddToInventory();
            }
        }
        public override void Interact()
        {
            if (IsInteracted) return;
            if (inventoryItemData.Interactable)
            {
                AddToInventory();
            }
        }
        private void AddToInventory()
        {
            IsInteracted = true;
            fadeAnimation.OnAnimationEnd.AddListener(() => { Destroy(gameObject); });
            fadeAnimation.StartAnimation();
            InventoryManager.Instance.AddToInventory(InventoryItem, ItemAmount);
            itemCollider.enabled = false;
        }

        public void SetData(InventoryItem inventoryItem, int count)
        {
            if (inventoryItem == null) return;

            inventoryItemData = inventoryItem;
            itemSprite.sprite = inventoryItem.ItemSprite;
            if (inventoryItem.Interactable)
            {
                gameObject.tag = "Interactable";
            }

            int layer = inventoryItem.LayerMask;
            gameObject.layer = layer;
            if (inventoryItem.CanBeStacked)
            {
                itemAmount = count;
            }

            itemCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        public void MoveTo(Vector3 end)
        {
            transformAnimation = new TransformAnimation(transform, .1f, end);
            transformAnimation.Init((IEnumerator enumerator) => { StartCoroutine(enumerator); });

            transformAnimation.StartAnimation();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (inventoryItemData != null)
            {
                itemSprite.sprite = inventoryItemData.ItemSprite;
                itemAmount = inventoryItemData.Amount;
            }
        }
        [ContextMenu("Reset data")]
        public void ResetData()
        {
            inventoryItemData = null;
            itemSprite.sprite = null;
            OnValidate();
        }
#endif
    }
}
