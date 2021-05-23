using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using InventorySystem;
using InteractionSystem;
public class EntityDropManager : MonoBehaviour
{
    [SerializeField]
    private List<ItemDropData> itemDropDatas = new List<ItemDropData>();

    [SerializeField]
    private int maxItemDropCount = 2;

    [SerializeField]
    private int maxOneItemDropAmount = 10;

    [SerializeField]
    private GameObject dropItemPrefab;

    [SerializeField]
    private GameObject coinPrefab;

    public void Drop()
    {
        float dropChance = UnityEngine.Random.Range(0f, 1f);
        List<ItemDropData> openList = new List<ItemDropData>();

        foreach(ItemDropData dropData in itemDropDatas)
        {
            if(dropData.DropChance > dropChance || dropData.DropChance == 1)
            {
                openList.Add(dropData);
            }
        }

        int dropCount = UnityEngine.Random.Range(1, maxItemDropCount);
        dropCount = dropCount > openList.Count ? openList.Count : dropCount;
        for (int i = 0; i < dropCount; i++)
        {
            int dropItemId = UnityEngine.Random.Range(0, openList.Count);
            int itemDropCount = UnityEngine.Random.Range(1, maxOneItemDropAmount);

            ItemDropData itemDrop = openList[dropItemId];

            GameObject drop = Instantiate(dropItemPrefab);
            drop.transform.position = transform.position;
            InventoryPickableItem pickableItem = drop.GetComponent<InventoryPickableItem>();
            pickableItem.SetDataSideScroller(itemDrop.Item, itemDropCount);

            openList.Remove(itemDrop);
        }
        GameObject coin = Instantiate(coinPrefab);
        coin.transform.position = transform.position;
    }

    [Serializable]
    public sealed class ItemDropData
    {
        [SerializeField]
        [Range(0, 1)]
        private float dropChance = 1;
        [SerializeField]
        private InventoryItem item;

        public float DropChance => dropChance;
        public InventoryItem Item => item;
    }
}
