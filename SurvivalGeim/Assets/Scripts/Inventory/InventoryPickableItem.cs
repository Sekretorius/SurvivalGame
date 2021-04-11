using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using UnityEngine.UI;
using AnimationSystem;
using System;

public class InventoryPickableItem : PickableItem
{
    [SerializeField]
    private InventoryItem inventoryItemData;
    [SerializeField]
    private SpriteRenderer itemSprite;
    [SerializeField]
    private int itemAmount;
    [SerializeField]
    private FadeAnimation fadeAnimation;

    [SerializeField]
    private Image pickButtonPosition;

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
        if (IsPicked) return;
        if (!inventoryItemData.Interactable && !collision.collider.isTrigger && collision.collider.name.Equals("Player"))
        {
            AddToInventory();
        }
    }
    public override void Interact()
    {
        if (IsPicked) return;
        if (inventoryItemData.Interactable)
        {
            AddToInventory();
        }
    }
    private void AddToInventory()
    {
        IsPicked = true;
        fadeAnimation.OnAnimationEnd.AddListener(() => { Destroy(gameObject); });
        fadeAnimation.StartAnimation();
        InventoryManager.Instance.AddToInventory(this);
        itemCollider.enabled = false;
    }
    public void SetInteractionState(bool state)
    {
        pickButtonPosition?.gameObject.SetActive(state);
    }

    private void OnValidate()
    {
        if (inventoryItemData != null) 
        {
            itemSprite.sprite = inventoryItemData.ItemSprite;
            itemAmount = inventoryItemData.Amount;
        }
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
        int layerNumber = 0;
        int layer = inventoryItem.LayerMask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        gameObject.layer = layerNumber-1;
        itemAmount = count;

        itemCollider = gameObject.AddComponent<BoxCollider2D>();
    }
}
