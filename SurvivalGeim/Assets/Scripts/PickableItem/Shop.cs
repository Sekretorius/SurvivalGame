using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI priceField;
    [SerializeField]
    private ShopItem firstSelected;
    private ShopItem selectedShopItem;


    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip buy_sound;


    private void Start()
    {
        if (selectedShopItem == null)
        {
            OnItemButtonClick(firstSelected);
        }
    }
    private void OnEnable()
    {
        InventoryManager.Instance.gameObject.SetActive(false);
        OnItemButtonClick(firstSelected);
    }
    private void OnDisable()
    {
        InventoryManager.Instance.gameObject.SetActive(true);
        selectedShopItem?.OnSelect(false);
        selectedShopItem = null;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TopDownPlayerController.Instance.UnFreezeMovement();
            gameObject.SetActive(false);
        }
    }
    public void OnItemButtonClick(ShopItem shopItem)
    {
        selectedShopItem?.OnSelect(false);
        shopItem?.OnSelect(true);

        selectedShopItem = shopItem;
        priceField.text = shopItem.Price.ToString();
    }
    public void OnBuyButtonClick()
    {
        if(selectedShopItem != null && InventoryManager.Instance != null && PlayerManager.instance != null)
        {
            if (PlayerManager.instance.money >= selectedShopItem.Price)
            {
                audioSource.PlayOneShot(buy_sound);
                PlayerManager.instance.ChangeMoney(-selectedShopItem.Price);
                InventoryManager.Instance.AddToInventory(selectedShopItem.Item, 1);
            }
        }
    }
}
