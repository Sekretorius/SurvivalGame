using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using UnityEngine.UI;
using AnimationSystem;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private InventoryItem item;
    [SerializeField]
    private int price;

    [SerializeField]
    private InventorySlot inventorySlot;

    public InventoryItem Item => item;
    public int Price => price;
    private void Start()
    {
        if (inventorySlot.InventoryItem == null)
        {
            inventorySlot.InventoryItem = item;
        }
    }
    private void OnEnable()
    {
        inventorySlot.InventoryItem = item;
    }
    public void OnSelect(bool state)
    {
        inventorySlot.SetSelection(state);
    }

    [ContextMenu("Fit")]
    public void Fit()
    {
        GetComponent<Image>().rectTransform.CalculateAnchors();
    }
}
