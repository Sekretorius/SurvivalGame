using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventorySystem
{
    [Serializable]
    [CreateAssetMenu(fileName = "InventoryConsumableItem", menuName = "Inventory/InventoryConsumableItem", order = 3)]
    public class InventoryConsumableItem : InventoryItem
    {
        [SerializeField]
        private List<ItemEffect> effects = new List<ItemEffect>();

        public List<ItemEffect> Effects => effects;

        public void Consume()
        {
            foreach (ItemEffect effect in effects)
            {
                effect.ApplyEffect();
            }
        }
    }
}