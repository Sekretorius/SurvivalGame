using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Mask))]
public class ItemArea : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    //[SerializeField]
    //private RectTransform cell;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Sprite ItemSprite;


    [SerializeField]
    private Vector2 cellBounds;

    private ItemCellPositionMap itemCellPositionMap = new ItemCellPositionMap();
    private Dictionary<int, List<Transform>> cellTransforms = new Dictionary<int, List<Transform>>();

    private void Start()
    {
        cellBounds = new Vector2(InventoryItemArea.Instance.CellSizeWidth, InventoryItemArea.Instance.CellSizeHeight);
        CalculateCells();
    }
    private void CalculateCells()
    {
        if (rectTransform == null || cellBounds == Vector2.zero)
        {
            return;
        }
        float xDiff = rectTransform.rect.width / cellBounds.x;
        float yDiff = rectTransform.rect.height / cellBounds.y;

        int xCellCount = Mathf.CeilToInt(xDiff);
        int yCellCount = Mathf.CeilToInt(yDiff);

        Vector2 topLeftLocalCorner = new Vector2(-xCellCount * cellBounds.x / 2, yCellCount * cellBounds.y / 2);

        if(cellTransforms.Count > 0)
        {
            List<int> keys = new List<int>(cellTransforms.Keys);
            foreach(int key in keys)
            {
                for(int i = 0; i < cellTransforms[key].Count; i++)
                {
                    if (cellTransforms[key][i] != null)
                    {
                        DestroyImmediate(cellTransforms[key][i].gameObject);
                    }
                }
            }
        }


        cellTransforms = new Dictionary<int, List<Transform>>();

        for (int i = 0; i < yCellCount; i++)
        {
            cellTransforms.Add(i, new List<Transform>());
            for (int j = 0; j < xCellCount; j++)
            {
                GameObject newCell = new GameObject(i + "_CellPosition", typeof(AttachedCellController));
                newCell.layer = LayerMask.NameToLayer("InventoryItemCell");

                RectTransform rectT = newCell.AddComponent<RectTransform>();
                Image cellImage = newCell.AddComponent<Image>();
                Rigidbody2D rgbd = newCell.AddComponent<Rigidbody2D>();
                
                rgbd.isKinematic = true;
                rgbd.useFullKinematicContacts = true;
                cellImage.color = Color.red;

                rectT.sizeDelta = cellBounds;
                newCell.transform.SetParent(transform);
                rectT.localScale = Vector3.one;
                newCell.transform.localPosition = new Vector2(topLeftLocalCorner.x + cellBounds.x / 2 + cellBounds.x * j, topLeftLocalCorner.y - cellBounds.y / 2 - cellBounds.y * i);

                cellTransforms[i].Add(newCell.transform);
            }
        }
    }
#if UNITY_EDITOR
    [ContextMenu("Test/calculate cells")]
    public void Test()
    {
        CalculateCells();
    }
    [ContextMenu("Test/Save new item")]
    public void SaveAsset()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Recources"))
        {
            AssetDatabase.CreateFolder("Assets", "Recources");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Recources/Item Data"))
        {
            AssetDatabase.CreateFolder("Assets/Recources", "Item Data");
        }

        InventoryItemData newData = ScriptableObject.CreateInstance<InventoryItemData>();
        itemCellPositionMap = new ItemCellPositionMap();
        foreach (int key in cellTransforms.Keys) {
            foreach (Transform cellTransform in cellTransforms[key])
            {
                if (cellTransform == null) continue;
                itemCellPositionMap.CellPositions.Add(new ItemCellPosition(key, cellTransform.localPosition));
            }
        }


        newData.SetData(gameObject.name, ItemSprite, rectTransform.sizeDelta, itemCellPositionMap);
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Recources/Item Data/ItemData.asset");
        AssetDatabase.CreateAsset(newData, path);
        AssetDatabase.SaveAssets();
    }
#endif
}
