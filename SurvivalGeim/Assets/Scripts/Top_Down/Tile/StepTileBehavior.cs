using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTileBehavior : TileBehavior
{
    [SerializeField]
    private HeightChangeType heightChangeType = HeightChangeType.Up;
    protected override void TileEffector(CharacterEventHandler controller)
    {
        TilemapHeightManager.Instance.ChangeLevelHeight(heightChangeType);
    }
}
