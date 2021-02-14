using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public enum HeightChangeType { Down, Up }
public class TilemapHeightManager : MonoBehaviour
{
    public static TilemapHeightManager Instance { get; private set; }
    [SerializeField]
    private List<TilemapHeightData> tilemapHeightDatas = new List<TilemapHeightData>();
    [SerializeField]
    private int currentHeightLevel = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        foreach(TilemapHeightData data in tilemapHeightDatas)
        {
            data.Init();
        }
    }
    public void ChangeLevelHeight(HeightChangeType direction)
    {
        switch (direction)
        {
            case HeightChangeType.Down:
                currentHeightLevel--;
                UpdateTilemap();
                break;
            case HeightChangeType.Up:
                currentHeightLevel++;
                UpdateTilemap();
                break;
        }
    }
    private void UpdateTilemap()
    {
        foreach(TilemapHeightData data in tilemapHeightDatas)
        {
            data.SetColliderStates(data.HeightLevel == currentHeightLevel);
        }
    }
}

[Serializable]
public class TilemapHeightData
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private int heightLevel = 0;
    [SerializeField]
    private List<Collider2D> collider2Ds = new List<Collider2D>();

    public int HeightLevel => heightLevel;
    public void Init()
    {
        collider2Ds = tilemap.GetComponents<Collider2D>().ToList();
    }
    public void SetColliderStates(bool state)
    {
        foreach(Collider2D collider in collider2Ds)
        {
            collider.enabled = state;
        }
    }
}