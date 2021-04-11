using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventorySystem
{
    [Serializable]
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/InventoryItemDatabase", order = 0)]
    public class InventoryItemDataBase : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItem> inventoryItems = new List<InventoryItem>();


        private List<InventoryEquiptableItem> inventoryEquiptables = new List<InventoryEquiptableItem>();
        private List<InventoryConsumableItem> inventoryConsumables = new List<InventoryConsumableItem>();
        private List<InventoryItem> basicItems = new List<InventoryItem>();

        public List<InventoryItem> InventoryItems => inventoryItems;
        public List<InventoryEquiptableItem> InventoryEquiptables => inventoryEquiptables;
        public List<InventoryConsumableItem> InventoryConsumables => inventoryConsumables;
        public List<InventoryItem> BasicItems => basicItems;


        private void OnValidate()
        {
            inventoryEquiptables.Clear();
            inventoryConsumables.Clear();
            basicItems.Clear();
            foreach (InventoryItem item in inventoryItems)
            {
                if (item is InventoryEquiptableItem)
                {
                    inventoryEquiptables.Add((InventoryEquiptableItem)item);
                }
                else if (item is InventoryConsumableItem)
                {
                    inventoryConsumables.Add((InventoryConsumableItem)item);
                }
                else
                {
                    basicItems.Add(item);
                }
            }
        }
    }
}