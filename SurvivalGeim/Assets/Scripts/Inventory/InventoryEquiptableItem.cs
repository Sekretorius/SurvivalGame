using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventorySystem
{
    public enum EquiptableType { Nothing, Weapon, BodyArmor, Shoes, Helmet, Accessory }
    [Serializable]
    [CreateAssetMenu(fileName = "InventoryEquiptableItem", menuName = "Inventory/InventoryEquiptableItem", order = 2)]
    public class InventoryEquiptableItem : InventoryItem
    {
        [SerializeField]
        private List<ItemEffect> effects = new List<ItemEffect>();
        [SerializeField]
        private EquiptableType equiptableType;

        public List<ItemEffect> Effects => effects;
        public EquiptableType EquiptableType => equiptableType;
    }
}