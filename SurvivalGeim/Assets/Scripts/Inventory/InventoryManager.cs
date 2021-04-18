using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using InteractionSystem;

namespace InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [SerializeField]
        private GameObject palyerInvetoryView;

        [SerializeField]
        private Image inventoryMainFrame;
        [SerializeField]
        private Image inventoryEquiptableFrame;

        [SerializeField]
        private List<InventorySlot> inventorySlots = new List<InventorySlot>();

        [SerializeField]
        private List<InventorySlot> equiptableSlots = new List<InventorySlot>();

        [SerializeField]
        private Sprite pickButtonSprite;

        [SerializeField]
        private GameObject itemDropPrefab;

        private Dictionary<int, InventorySlot> inventoryItemSlots = new Dictionary<int, InventorySlot>();
        private Dictionary<int, InventorySlot> equiptableItemSlots = new Dictionary<int, InventorySlot>();

        private int selectedSlotId = -1;
        private int selectedEquiptableSlotId = -1;

        public Sprite PickButtonSprite => pickButtonSprite;

        [SerializeField]
        private bool isSavingEnabled = false;
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

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                int nextId = selectedSlotId + 1 < inventoryItemSlots.Count ? selectedSlotId + 1 : 0;
                SelectSlot(nextId);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                int nextId = selectedSlotId - 1 >= 0 ? selectedSlotId - 1 : inventoryItemSlots.Count - 1;
                SelectSlot(nextId);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                SetEquiptableInventoryState(!palyerInvetoryView.activeInHierarchy);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetEquiptableInventoryState(false);
            }
            if (Input.GetKeyDown(KeyCode.Q) && selectedSlotId >= 0)
            {
                DropItem(inventoryItemSlots[selectedSlotId]);
            }
            if (Input.GetMouseButtonDown(0))
            {
                CheckSlot(Input.mousePosition);
            }

            if (TopDownPlayerController.Instance != null)
            {
                CheckInventoryUICollision(TopDownPlayerController.Instance.gameObject.transform.position);
            }
            if (PlayerController.instance != null)
            {
                CheckInventoryUICollision(PlayerController.instance.gameObject.transform.position);
            }

            if (Input.GetKey(KeyCode.S))
            {
                SaveData();
                LoadData();
            }
        }

#region Save system
        private const string MainSaveKey = "_InventorySave";
        private const string NotEquiptedItemCountKey = "_notEquiptedCount";
        private const string EquiptedItemCountKey = "_notEquiptedCount";
        private const string SlotSaveKey = "_slot_";
        private int saveId = 0;
        public string InventorySaveKey 
        { 
            get
            {
                return saveId + MainSaveKey;
            } 
        }
        public void SaveData()
        {
            int notEquiptedItemCount = 0; 
            int equiptedItemCount = 0;
            int slotId = 0;
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.InventoryItem != null)
                {
                    InventoryItemSave inventoryItemSave = new InventoryItemSave(slot, SlotType.MainInventorySlot, slotId);
                    string jsonLine = JsonUtility.ToJson(inventoryItemSave);
                    PlayerPrefs.SetString(InventorySaveKey + SlotSaveKey + slotId, jsonLine);
                    notEquiptedItemCount++;
                }
                slotId++;
            }
            foreach (InventorySlot slot in equiptableSlots)
            {
                if (slot.InventoryItem != null)
                {
                    InventoryItemSave inventoryItemSave = new InventoryItemSave(slot, SlotType.MainInventorySlot, slotId);
                    string jsonLine = JsonUtility.ToJson(inventoryItemSave);
                    PlayerPrefs.SetString(InventorySaveKey + SlotSaveKey + slotId, jsonLine);
                    equiptedItemCount++;
                }
                slotId++;
            }

            PlayerPrefs.SetInt(InventorySaveKey + NotEquiptedItemCountKey, notEquiptedItemCount);
            PlayerPrefs.SetInt(InventorySaveKey + EquiptedItemCountKey, equiptedItemCount);
            PlayerPrefs.Save();
        }
        public void LoadData()
        {
            int notEquiptedItemCount = PlayerPrefs.GetInt(InventorySaveKey + NotEquiptedItemCountKey, 0);
            int equiptedItemCount = PlayerPrefs.GetInt(InventorySaveKey + EquiptedItemCountKey, 0);
            int slotId = 0;
            List<InventoryItemSave> mainSlots = new List<InventoryItemSave>();
            List<InventoryItemSave> equiptableSlots = new List<InventoryItemSave>();

            for (int i = 0; i < notEquiptedItemCount; i++)
            {
                string saveJsonLine = PlayerPrefs.GetString(InventorySaveKey + SlotSaveKey + slotId, null);
                slotId++;
                if (saveJsonLine == null) continue;

                InventoryItemSave savedData = JsonUtility.FromJson<InventoryItemSave>(saveJsonLine);
                mainSlots.Add(savedData);
            }
            for (int i = 0; i < equiptedItemCount; i++)
            {
                string saveJsonLine = PlayerPrefs.GetString(InventorySaveKey + SlotSaveKey + slotId, null);
                slotId++;
                if (saveJsonLine == null) continue;

                InventoryItemSave savedData = JsonUtility.FromJson<InventoryItemSave>(saveJsonLine);
                equiptableSlots.Add(savedData);
            }
        }

        internal enum SlotType { MainInventorySlot, EquiptableSlot}
        [Serializable]
        internal class InventoryItemSave
        {
            public int SlotId = 0;
            public InventoryItem InventoryItem;
            public int ItemCount;
            public SlotType SlotType;
            public InventoryItemSave(InventorySlot slot, SlotType slotType, int slotId)
            {
                InventoryItem = slot.InventoryItem;
                ItemCount = slot.ItemCount;
                SlotType = slotType;
                SlotId = slotId;
            }
        }
#endregion
        public void CheckInventoryUICollision(Vector3 worldPosition)
        {
            Vector2 offset = new Vector2(0.5f, 0.5f);
            List<Vector2> screenPoints = new List<Vector2>()
            {
                Camera.main.WorldToScreenPoint(new Vector2(worldPosition.x + offset.x, worldPosition.y + offset.y)),
                Camera.main.WorldToScreenPoint(new Vector2(worldPosition.x - offset.x, worldPosition.y - offset.y)),
                Camera.main.WorldToScreenPoint(new Vector2(worldPosition.x - offset.x, worldPosition.y + offset.y)),
                Camera.main.WorldToScreenPoint(new Vector2(worldPosition.x + offset.x, worldPosition.y - offset.y))
            };

            foreach (Vector2 screenPoint in screenPoints) {

                if (RectTransformUtility.RectangleContainsScreenPoint(inventoryMainFrame.rectTransform, screenPoint))
                {
                    FadeInventory(true);
                    return;
                }
            }
            FadeInventory(false);
        }

        public void FadeInventory(bool isFadingIn)
        {
            float alpha = 1;
            if (isFadingIn)
            {
                alpha = 0.25f;
            }

            foreach (InventorySlot slot in inventoryItemSlots.Values)
            {
                slot.SetAlpha(alpha);
            }
            foreach (InventorySlot slot in equiptableItemSlots.Values)
            {
                slot.SetAlpha(alpha);
            }

            inventoryMainFrame.color = new Color(inventoryMainFrame.color.r, inventoryMainFrame.color.g, inventoryMainFrame.color.b, alpha);
            inventoryEquiptableFrame.color = new Color(inventoryEquiptableFrame.color.r, inventoryEquiptableFrame.color.g, inventoryEquiptableFrame.color.b, alpha);

        }

        public void SetEquiptableInventoryState(bool state)
        {
            palyerInvetoryView.SetActive(state);
            if (palyerInvetoryView.activeInHierarchy)
            {
                if (selectedEquiptableSlotId >= 0)
                {
                    equiptableItemSlots[selectedEquiptableSlotId].SetSelection(false);
                    selectedEquiptableSlotId = -1;
                }
            }
        }
        public void CheckSlot(Vector2 screenPoint)
        {
            foreach(int slotId in inventoryItemSlots.Keys)
            {
                if(inventoryItemSlots[slotId] != null)
                {
                    RectTransform imageTransform = inventoryItemSlots[slotId].SelectionField;

                    if(RectTransformUtility.RectangleContainsScreenPoint(imageTransform, screenPoint))
                    {
                        SelectSlot(slotId);
                        return;
                    }
                }
            }
            foreach (int slotId in equiptableItemSlots.Keys)
            {
                if (equiptableItemSlots[slotId] != null)
                {
                    RectTransform imageTransform = equiptableItemSlots[slotId].SelectionField;

                    if (RectTransformUtility.RectangleContainsScreenPoint(imageTransform, screenPoint))
                    {
                        if (selectedEquiptableSlotId != slotId)
                        {
                            if (selectedEquiptableSlotId != -1)
                            {
                                equiptableItemSlots[selectedEquiptableSlotId].SetSelection(false);
                            }
                            selectedEquiptableSlotId = slotId;
                            equiptableItemSlots[slotId].SetSelection(true);
                        }
                        else
                        {
                            if (equiptableItemSlots[selectedEquiptableSlotId].InventoryItem != null)
                            {
                                AddToInventory(equiptableItemSlots[selectedEquiptableSlotId].InventoryItem, equiptableItemSlots[selectedEquiptableSlotId].ItemCount);
                                ReleaseSlot(equiptableItemSlots[selectedEquiptableSlotId]);
                            }
                        }
                        return;
                    }
                }
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

        public void AddToInventory(InventoryItem inventoryItem, int amount, int slotId = 0)
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
            inventorySlot.InventoryItem = inventoryItem;
            inventorySlot.ItemCount = amount;
        }

        public void AddToInventory(InventoryItem inventoryItem, int amount)
        {
            int firstFreeSlotId = inventoryItemSlots.Count;

            for(int i = 0; i < inventoryItemSlots.Keys.Count; i++)
            {
                InventorySlot slot = inventoryItemSlots[i];
                if (inventoryItem.CanBeStacked && slot.InventoryItem != null && slot.InventoryItem.ItemName == inventoryItem.ItemName)
                {
                    slot.ItemCount += amount;
                    return;
                }
                if(slot.InventoryItem == null && i < firstFreeSlotId)
                {
                    firstFreeSlotId = i;
                }
            }
            if (inventoryItemSlots[selectedSlotId].InventoryItem != null && firstFreeSlotId != inventoryItemSlots.Count)
            {
                AddToInventory(inventoryItem, amount, firstFreeSlotId);
            }
            else
            {
                AddToInventory(inventoryItem, amount, selectedSlotId);
            }
        }

        public void DropItem(InventorySlot inventorySlot)
        {
            if (inventorySlot.InventoryItem != null)
            {
                InventoryPickableItem pickableItem = Instantiate(itemDropPrefab).GetComponent<InventoryPickableItem>();
                if (TopDownPlayerController.Instance != null)
                {
                    if(inventorySlot.InventoryItem.LayerMask == 10)
                    {
                        //to do: find empty position from player position
                        pickableItem.transform.position = TopDownPlayerController.Instance.transform.position;
                    }
                }
                else if (PlayerController.instance != null)
                {
                    //to do: ??? drop item in side scroller
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