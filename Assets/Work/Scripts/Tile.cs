
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Coord TileCoord { get; private set; }
    public Block InBlock { get; private set; }

    public void SetCoord(int _x, int _y)
    {
        TileCoord = new Coord(_x, _y);
    }
    public void SetBlock(Block _block)
    {
        InBlock = _block;
    }
}