using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationSystem;

namespace InteractionSystem
{
    public class InteractableChest : Interactable
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private GameObject itemDropPrefab;
        [SerializeField]
        private GameObject coinPrefab;


        [SerializeField]
        private List<InventoryItem> dropableItems = new List<InventoryItem>();
        [SerializeField]
        private int maxDropCount = 3;
        [SerializeField]
        private bool canDropCoin = false;

        private List<InventoryItem> openList = new List<InventoryItem>();

        [SerializeField]
        private Vector3 offset = Vector3.zero;
        [SerializeField]
        private FadeAnimation fadeAnimation;
        [SerializeField]
        private int layerMask;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip open_sound;
        protected override void Start()
        {
            base.Start();
            fadeAnimation.Init((IEnumerator enumerator) => { StartCoroutine(enumerator); });
            fadeAnimation.SpriteRenderer = GetComponent<SpriteRenderer>();

        }
        public override void Interact()
        {
            if (IsInteracted) return;
            IsInteracted = true;

            animator.SetTrigger("Open");
            audioSource.PlayOneShot(open_sound);

            openList = new List<InventoryItem>(dropableItems);

            int count = Random.Range(0, Mathf.Min(maxDropCount + 1, openList.Count));

            for(int i = 0; i <= count; i++)
            {
                InventoryItem item = openList[Random.Range(0, openList.Count)];
                openList.Remove(item);
                int itemCount = Random.Range(1, 4);

                InventoryPickableItem pickableItem = Instantiate(itemDropPrefab).GetComponent<InventoryPickableItem>();
                pickableItem.SetData(item, itemCount);

                pickableItem.transform.position = transform.position + offset;
                Vector3 direction = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0);
                pickableItem.MoveTo(pickableItem.transform.position + direction);
            }
            if (canDropCoin && Random.Range(0, 2) == 1)
            {
                Coin pickableItem = Instantiate(coinPrefab).GetComponent<Coin>();
                pickableItem.transform.position = transform.position + offset;
            }


            gameObject.layer = layerMask;
            itemCollider.enabled = false;
            fadeAnimation.OnAnimationEnd.AddListener(() => { Destroy(gameObject); });
            fadeAnimation.StartAnimation();
        }
    }
}
