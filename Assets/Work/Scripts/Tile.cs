
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile
{
    public Coord TileCoord { get; private set; }
    public BlockDummyElement InBlock { get; private set; }
    public bool IsBatched { get { return InBlock != null; } }

    public void SetCoord(int _x, int _y)
    {
        TileCoord = new Coord(_x, _y);
    }
    public void SetBlock(BlockDummyElement _block,BlockInfo _blockInfo)
    {
        if (_block == null) return;

        InBlock = _block;
        InBlock.BlockBatch(_blockInfo);
    }
    public void RemoveBlock()
    {
        if (InBlock != null)
        {
            InBlock.DestroyBlock();
            InBlock = null;
        }
    }
}