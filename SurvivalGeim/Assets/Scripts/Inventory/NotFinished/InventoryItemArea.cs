using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationSystem;

public class InventoryItemArea : MonoBehaviour
{
    public static InventoryItemArea Instance { get; private set; }
    [SerializeField]
    private RectTransform itemArea;
    [SerializeField]
    private int cellTargetCount;
    [SerializeField]
    private Canvas canvas;

    private float itemAreaWidth;
    private float itemAreaHeight;

    public float CellSizeWidth { get; set; }
    public float CellSizeHeight { get; set; }

    [SerializeField]
    private Dictionary<Collider2D, bool> itemAreas = new Dictionary<Collider2D, bool>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        Vector3[] corners = itemArea.GetScreenPointCornerPositions();
        itemAreaWidth = corners[2].x - corners[0].x;
        itemAreaHeight = corners[2].y - corners[0].y;

        float area = itemAreaWidth * itemAreaHeight;

        CellSizeWidth = Mathf.Sqrt(area / cellTargetCount);
        CellSizeHeight = Mathf.Sqrt(area / cellTargetCount);

        PopulateArea(corners, new Vector2(itemAreaWidth, itemAreaHeight) / canvas.scaleFactor, new Vector2(CellSizeWidth, CellSizeHeight) / canvas.scaleFactor);
    }
    public bool ReserveCell(Collider2D collider2D)
    {
        if (itemAreas.ContainsKey(collider2D) && !itemAreas[collider2D])
        {
            itemAreas[collider2D] = true;
            return true;
        }
        return false;
    }
    public void ReleaseCell(Collider2D collider2D)
    {
        if (itemAreas.ContainsKey(collider2D))
        {
            itemAreas[collider2D] = false;
        }
    }
    private void PopulateArea(Vector3[] areaCorners, Vector2 areaBounds, Vector2 cellSize)
    {
        int row = Mathf.FloorToInt(areaBounds.y / cellSize.y);
        int col = Mathf.FloorToInt(areaBounds.x / cellSize.x);

        float xOffset = cellSize.x / 2 * canvas.scaleFactor + (areaBounds.x - cellSize.x * col) / 2 * canvas.scaleFactor;
        float yOffset = cellSize.y / 2 * canvas.scaleFactor + (areaBounds.y - cellSize.y * row) / 2 * canvas.scaleFactor;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                GameObject newCell = new GameObject(i + " - " + j + " _Cell");
                BoxCollider2D boxCollider2D = newCell.AddComponent<BoxCollider2D>();
                newCell.transform.SetParent(transform);

                RectTransform rectT = newCell.AddComponent<RectTransform>();
                rectT.localScale = Vector2.one;
                rectT.sizeDelta = cellSize;
                rectT.position = new Vector2(areaCorners[0].x + xOffset + cellSize.x * j * canvas.scaleFactor, areaCorners[0].y + yOffset + cellSize.y * i * canvas.scaleFactor);

                boxCollider2D.size = rectT.rect.size;

                if (!itemAreas.ContainsKey(boxCollider2D))
                {
                    itemAreas.Add(boxCollider2D, false);
                }
            }
        }
    }
#if UNITY_EDITOR
    [ContextMenu("RectTransfom/CalculateAnchors")]
    public void CalculateAnchors()
    {
        if (itemArea != null)
        {
            itemArea.CalculateAnchors();
        }
    }
#endif
}
