
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile
{
    public Coord TileCoord { get; private set; }
    public BlockElement InBlock { get; private set; }

    public void SetCoord(int _x, int _y)
    {
        TileCoord = new Coord(_x, _y);
    }
    public void SetBlock(BlockElement _block)
    {
        InBlock = _block;
    }
}