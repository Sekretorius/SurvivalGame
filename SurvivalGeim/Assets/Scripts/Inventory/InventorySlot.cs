using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AnimationSystem;
using InventorySystem;

namespace InventorySystem
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField]
        private Image slotItemImage;
        [SerializeField]
        private Image selectionImage;
        [SerializeField]
        private TextMeshProUGUI amountText;

        [Tooltip("Items that can be equipted to this slot")]
        [SerializeField]
        private EquiptableType equiptableType;

        private int itemCount = 0;

        public EquiptableType EquiptableType => equiptableType; 
        public int ItemCount { 
            get 
            { 
                return itemCount; 
            } 
            set 
            { 
                itemCount = value;
                if (amountText != null)
                {
                    if (itemCount > 1)
                    {
                        amountText.text = itemCount.ToString();
                    }
                    else
                    {
                        amountText.text = "";
                    }
                }
            } 
        }

        private InventoryItem inventoryItem;
        public InventoryItem InventoryItem 
        { 
            get 
            {
                return inventoryItem;
            }
            set 
            { 
                if(value != null)
                {
                    slotItemImage.enabled = true;
                    slotItemImage.sprite = value.ItemSprite;
                    slotItemImage.preserveAspect = true;
                    itemCount += value.Amount;                   
                }
                else
                {
                    slotItemImage.sprite = null;
                    slotItemImage.enabled = false;
                    itemCount = 0;
                    if (amountText != null)
                    {
                        amountText.text = "";
                    }
                }
                inventoryItem = value;
            }
        }
        private void Awake()
        {
            slotItemImage.enabled = false;
            InventoryItem = null;
            selectionImage?.gameObject.SetActive(false);
        }
        public void SetSelection(bool state)
        {
            selectionImage?.gameObject.SetActive(state);
        }
        [ContextMenu("Fit")]
        public void Fit()
        {
            slotItemImage.rectTransform.CalculateAnchors();
        }
    }
}
