using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventorySystem
{
    [Serializable]
    [CreateAssetMenu(fileName = "InvetoryItem", menuName = "Inventory/InventoryItem", order = 1)]
    public class InventoryItem : ScriptableObject
    {
        [SerializeField]
        private string itemName;
        [SerializeField]
        private float weight = 0;
        [SerializeField]
        private int amount = 1;
        [SerializeField]
        private Sprite itemSprite;
        [SerializeField]
        private bool canBeStacked = false;
        [SerializeField]
        private bool isPicked = false;
        [SerializeField]
        [Min(0)]
        private int layerMask = 10;

        [SerializeField]
        private Vector3 spriteSize = Vector3.one;

        public string ItemName => itemName;
        public float Weight => weight;
        public int Amount => amount;
        public Sprite ItemSprite => itemSprite;
        public bool CanBeStacked => canBeStacked;
        public bool Interactable => isPicked;
        public int LayerMask => layerMask;
        public Vector3 SpriteSize => spriteSize;
    }


    public enum EffectType { Health, Mana, Speed, MeleeDamage, RangeDamage, Armor, Dodge, ManaRegen, HealthRegen }

    [Serializable]
    public class ItemEffect
    {
        [SerializeField]
        private EffectType effectType;
        [SerializeField]
        private float effectValue = 0;

        public EffectType EffectType => effectType;
        public float EffectValue => effectValue;

        //bad method not reusable...
        public void ApplyEffect()
        {
            if(PlayerManager.instance != null)
            {
                switch (effectType)
                {
                    case EffectType.Health:
                        PlayerManager.instance.ChangeHealth(Mathf.FloorToInt(effectValue));
                        break;
                    case EffectType.Mana:
                        PlayerManager.instance.ChangeMana(Mathf.FloorToInt(effectValue));
                        break;
                }

            }
        }
    }
}