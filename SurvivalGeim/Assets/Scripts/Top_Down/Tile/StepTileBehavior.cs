using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTileBehavior : TileBehavior
{
    [SerializeField]
    private HeightChangeType heightChangeType = HeightChangeType.Up;

    [SerializeField]
    private StepTileBehavior connectingTile;
    public bool isTriggerTile { get; set; } = false;
    protected override void TileEffector(CharacterEventHandler controller, bool state)
    {
        if (connectingTile.IsSteppedByPlayer && !connectingTile.isTriggerTile && state)
        {
            connectingTile.isTriggerTile = true;
            TilemapHeightManager.Instance.ChangeLevelHeight(connectingTile.heightChangeType);
            RotateDirection();
            connectingTile.RotateDirection();
        }
        if (!state)
        {
            isTriggerTile = false;
        }
    }
    public void RotateDirection()
    {
        heightChangeType = heightChangeType == HeightChangeType.Down ? HeightChangeType.Up : HeightChangeType.Down;
    }
}
