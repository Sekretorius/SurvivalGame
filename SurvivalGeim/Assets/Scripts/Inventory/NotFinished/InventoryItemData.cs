using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="InventoryItem", menuName ="Inventory/Item", order = 1)]
public class InventoryItemData : ScriptableObject
{
    [SerializeField]
    private string title;
    [SerializeField]
    private Sprite itemSprite;
    [SerializeField]
    private Vector2 itemSize;
    [SerializeField]
    private Vector2 cellSize;

    [SerializeField]
    private ItemCellPositionMap cellPositionMap;

    public string Title => title;
    public Sprite ItemSprite => itemSprite;
    public Vector2 Itemsize => itemSize;
    public Vector2 CellSize => cellSize;
    public ItemCellPositionMap CellPositionMap => cellPositionMap;


    public void SetData(string title, Sprite itemSprite, Vector2 itemSize, ItemCellPositionMap cellPositionMap)
    {
        this.title = title;
        this.itemSprite = itemSprite;
        this.itemSize = itemSize;
        this.cellPositionMap = cellPositionMap;
    }
}

[Serializable]
public class ItemCellPositionMap
{
    [SerializeField]
    private List<ItemCellPosition> positions = new List<ItemCellPosition>();

    public List<ItemCellPosition> CellPositions => positions;

    public ItemCellPositionMap()
    {
        positions = new List<ItemCellPosition>();
    }
}
[Serializable]
public class ItemCellPosition
{
    [SerializeField]
    private int layer = 0;
    [SerializeField]
    private Vector2 position;

    public int Layer => layer;
    public Vector2 Position => position;

    public ItemCellPosition(int layer, Vector2 position)
    {
        this.layer = layer;
        this.position = position;
    }
}