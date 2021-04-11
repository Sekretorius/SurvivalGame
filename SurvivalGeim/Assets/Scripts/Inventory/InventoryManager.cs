using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [SerializeField]
        private GameObject palyerInvetoryView;

        [SerializeField]
        private List<InventorySlot> inventorySlots = new List<InventorySlot>();

        [SerializeField]
        private List<InventorySlot> equiptableSlots = new List<InventorySlot>();

        [SerializeField]
        private Sprite pickButtonSprite;

        [SerializeField]
        private GameObject itemDropPrefab;

        private List<KeyCode> slotKeys = new List<KeyCode>()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8
        };

        private Dictionary<int, InventorySlot> inventoryItemSlots = new Dictionary<int, InventorySlot>();
        private Dictionary<int, InventorySlot> equiptableItemSlots = new Dictionary<int, InventorySlot>();

        private Queue<InventoryPickableItem> pickableItems = new Queue<InventoryPickableItem>();

        private int selectedSlotId = -1;

        public Sprite PickButtonSprite => pickButtonSprite;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (!inventoryItemSlots.ContainsKey(i))
                {
                    inventoryItemSlots.Add(i, inventorySlots[i]);
                }
            }
            for (int i = 0; i < equiptableSlots.Count; i++)
            {
                if (!equiptableItemSlots.ContainsKey(i))
                {
                    equiptableItemSlots.Add(i, equiptableSlots[i]);
                }
            }
            palyerInvetoryView.SetActive(false);
            SelectSlot(0);
        }

        private void Update()
        {
            for(int i = 0; i < slotKeys.Count; i++)
            {
                if (Input.GetKeyDown(slotKeys[i]))
                {
                    SelectSlot(i);
                }
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                palyerInvetoryView.SetActive(!palyerInvetoryView.activeInHierarchy);
            }
            if (Input.GetKeyDown(KeyCode.Q) && selectedSlotId >= 0)
            {
                DropItem(inventoryItemSlots[selectedSlotId]);
            }
        }
        public void SelectSlot(int slotId)
        {
            if(inventoryItemSlots.ContainsKey(slotId))
            {
                if (selectedSlotId >= 0 && slotId == selectedSlotId)
                {
                    InventoryItem item = inventoryItemSlots[selectedSlotId].InventoryItem;
                    InventorySlot takenSlot = inventoryItemSlots[selectedSlotId];
                    if (item != null)
                    {
                        if (item is InventoryConsumableItem)
                        {
                            ((InventoryConsumableItem)item).Consume();
                            inventoryItemSlots[selectedSlotId].ItemCount--;
                            if (inventoryItemSlots[selectedSlotId].ItemCount <= 0)
                            {
                                ReleaseSlot(takenSlot);
                            }
                        }
                        if (item is InventoryEquiptableItem)
                        {
                            ReleaseSlot(takenSlot);
                            Equipt((InventoryEquiptableItem)item, takenSlot);
                        }
                    }
                }
                else if (slotId != selectedSlotId)
                {
                    if (selectedSlotId >= 0)
                    {
                        inventoryItemSlots[selectedSlotId].SetSelection(false);
                    }
                    inventoryItemSlots[slotId].SetSelection(true);
                    selectedSlotId = slotId;
                }
            }
        }

        public void Equipt(InventoryEquiptableItem item, InventorySlot releasedSlot)
        {
            item.Equipt();

            foreach (InventorySlot slot in equiptableItemSlots.Values)
            {
                if(slot.EquiptableType == item.EquiptableType)
                {
                    if(slot.InventoryItem != null)
                    {
                        releasedSlot.InventoryItem = slot.InventoryItem;
                    }
                    slot.InventoryItem = item;
                    break;
                }
            }
        }

        public void AddToInventory(InventoryPickableItem inventoryPickableItem, int slotId = 0)
        {
            InventorySlot inventorySlot;
            if (inventoryItemSlots.ContainsKey(slotId))
            {
                inventorySlot = inventoryItemSlots[slotId];
            }
            else
            {
                inventorySlot = inventoryItemSlots[0];
            }

            if (inventorySlot.InventoryItem != null)
            {
                DropItem(inventorySlot);
            }
            inventorySlot.InventoryItem = inventoryPickableItem.InventoryItem;
            inventorySlot.ItemCount = inventoryPickableItem.ItemAmount;
        }

        public void AddToInventory(InventoryPickableItem inventoryPickableItem)
        {
            int firstFreeSlotId = inventoryItemSlots.Count;

            for(int i = 0; i < inventoryItemSlots.Keys.Count; i++)
            {
                InventorySlot slot = inventoryItemSlots[i];
                if (inventoryPickableItem.InventoryItem.CanBeStacked && slot.InventoryItem != null && slot.InventoryItem.ItemName == inventoryPickableItem.InventoryItem.ItemName)
                {
                    slot.ItemCount += inventoryPickableItem.ItemAmount;
                    return;
                }
                if(slot.InventoryItem == null && i < firstFreeSlotId)
                {
                    firstFreeSlotId = i;
                }
            }
            if (inventoryItemSlots[selectedSlotId].InventoryItem != null && firstFreeSlotId != inventoryItemSlots.Count)
            {
                AddToInventory(inventoryPickableItem, firstFreeSlotId);
            }
            else
            {
                AddToInventory(inventoryPickableItem, selectedSlotId);
            }
        }

        public void DropItem(InventorySlot inventorySlot)
        {
            if (inventorySlot.InventoryItem != null)
            {
                InventoryPickableItem pickableItem = Instantiate(itemDropPrefab).GetComponent<InventoryPickableItem>();
                if (TopDownPlayerController.Instance != null)
                {
                    pickableItem.transform.position = TopDownPlayerController.Instance.transform.position;
                }
                else if (PlayerController.instance != null)
                {
                    //pickableItem.transform.position = PlayerController.instance.transform.position;
                }
                pickableItem.SetData(inventorySlot.InventoryItem, inventorySlot.ItemCount);
                ReleaseSlot(inventorySlot);
            }
        }

        public void ReleaseSlot(InventorySlot inventorySlot)
        {
            inventorySlot.InventoryItem = null;
        }
    }
}
