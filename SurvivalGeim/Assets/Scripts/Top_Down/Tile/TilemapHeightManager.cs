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
    private List<TilemapHeightContainer> tilemapContainers = new List<TilemapHeightContainer>();
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
        foreach (TilemapHeightContainer data in tilemapContainers)
        {
            data.Init(currentHeightLevel);
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
        foreach (TilemapHeightContainer data in tilemapContainers)
        {
            if(data.HeightLevel == currentHeightLevel)
            {
                TopDownPlayerController.Instance.UpdateSpriteLayer(data.SetPlayerLayerTo);
            }
            data.SetColliderStates(currentHeightLevel);
        }
    }
}

[Serializable]
public class TilemapHeightContainer
{
    [SerializeField]
    private string maplevelName = "";
    [SerializeField]
    private int setPlayerLayerTo = 0;
    [SerializeField]
    private int heightLevel = 0;
    [SerializeField]
    private List<TilemapHeightData> tilemapHeightDatas = new List<TilemapHeightData>();

    public string MaplevelName => maplevelName;
    public int HeightLevel => heightLevel;
    public int SetPlayerLayerTo => setPlayerLayerTo;

    public List<TilemapHeightData> TilemapHeightDatas => tilemapHeightDatas;

    public void Init(int currentHeight)
    {
        foreach (TilemapHeightData data in tilemapHeightDatas)
        {
            data.Init(currentHeight, heightLevel);
        }
    }

    public void SetColliderStates(int currentHeight)
    {
        foreach(TilemapHeightData data in tilemapHeightDatas)
        {
            data.SetColliderStates(currentHeight, heightLevel);
        }
    }
}

[Serializable]
public class TilemapHeightData
{
    [Header("Main data")]
    [SerializeField]
    private Tilemap tilemap;
    public Tilemap Tilemap => tilemap;

    [Header("Conditions")]
    [SerializeField]
    private Vector2 disableObjectInRange = Vector2.zero;
    [SerializeField]
    private Vector2 disableColliderInRange = Vector2.zero;
    [SerializeField]
    private bool canEffectCollider = true;
    [SerializeField]
    private bool canEffectObject = true;

    [Header("Tilemap colliders")]
    [SerializeField]
    private List<Collider2D> collider2Ds = new List<Collider2D>();

    public TilemapHeightData(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }

    public void Init(int currentHeight, int levelHeight)
    {
        collider2Ds = tilemap.GetComponents<Collider2D>().ToList();
        SetColliderStates(currentHeight, levelHeight);
    }
    public void SetColliderStates(int currentHeight, int levelHeight)
    {
        int distance = currentHeight - levelHeight;
        bool state = levelHeight == currentHeight;

        if (canEffectObject)
        {
            bool objectState = distance >= disableObjectInRange.x && distance <= disableObjectInRange.y;
            tilemap.gameObject.SetActive(state || objectState);
        }
        if (canEffectCollider)
        {
            bool colliderState = distance >= disableColliderInRange.x && distance <= disableColliderInRange.y;
            foreach (Collider2D collider in collider2Ds)
            {
                collider.enabled = state || colliderState;
            }
        }
    }
}