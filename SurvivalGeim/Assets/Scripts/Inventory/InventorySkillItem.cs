using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventorySystem
{
    [Serializable]
    [CreateAssetMenu(fileName = "InventorySkillItem", menuName = "Inventory/InventorySkillItem", order = 4)]
    public class InventorySkillItem : InventoryItem
    {
        public enum SkillType { Projectile, Skill }

        [SerializeField]
        private GameObject skillPrefab;
        [SerializeField]
        private SkillType skillType;
        public GameObject SkillPrefab => skillPrefab;
        public SkillType SkillT => skillType;
    }
}
