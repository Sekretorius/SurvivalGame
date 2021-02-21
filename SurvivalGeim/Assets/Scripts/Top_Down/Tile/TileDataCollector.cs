using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class TileDataCollector : MonoBehaviour
{
    [SerializeField]
    private TilemapHeightContainer container = new TilemapHeightContainer();
    public TilemapHeightContainer Container => container;

    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            ExistanceCheck();
            Asemble();
        }
    }
    private void ExistanceCheck()
    {
        for(int i = 0; i < container.TilemapHeightDatas.Count; i++)
        {
            TilemapHeightData data = container.TilemapHeightDatas[i];
            if (data.Tilemap == null || data.Tilemap.gameObject == null)
            {
                container.TilemapHeightDatas.Remove(data);
                i--;
            }
        }
    }
    private void Asemble()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out Tilemap data))
                {
                    TilemapHeightData newData = new TilemapHeightData(data);
                    if (!Contains(newData))
                    {
                        container.TilemapHeightDatas.Add(newData);
                    }
                }
            }
        }
    }
    private bool Contains(TilemapHeightData checkData)
    {
        foreach(TilemapHeightData data in container.TilemapHeightDatas)
        {
           if(data.Tilemap == checkData.Tilemap)
            {
                return true;
            }
        }
        return false;
    }
}
