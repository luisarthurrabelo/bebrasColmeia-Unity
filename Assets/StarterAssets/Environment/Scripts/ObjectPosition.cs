using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPosition : MonoBehaviour
{
    private Vector2 tile_position;

    public Vector2 TilePosition
    {
        get { return tile_position; }
        set { tile_position = value; }
    }
}
