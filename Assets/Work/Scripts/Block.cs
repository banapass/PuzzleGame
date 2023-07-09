using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public List<Vector2> BlockCoords { get; private set; }


    public void AddBlockCoords(Vector2 _coords)
    {
        if (BlockCoords == null) BlockCoords = new List<Vector2>();
        if (BlockCoords.Contains(_coords)) return;

        BlockCoords.Add(_coords);
    }



}
