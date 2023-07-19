using UnityEngine;
using UnityEngine.UIElements;

public class TileElement : VisualElement
{
    public Tile InTile { get; private set; }
    // public Coord[] BatchCoord { get; private set; }
    public TileElement(string _className, Tile _targetTile)
    {
        this.AddToClassList(_className);
        InTile = _targetTile;
        // BatchCoord = Coord.FourDirection;
    }
}

public class BlockElement : VisualElement
{
    public BlockInfo InBlockInfo { get; private set; }
    public Coord[] BatchCoord { get { return InBlockInfo.BlockCoord; } }

    public BlockElement()
    {

    }
    public BlockElement(string _className, BlockInfo _blockInfo)
    {
        if (_className != null) this.AddToClassList(_className);
        InBlockInfo = _blockInfo;
    }
}

public class BlockDummyElement : VisualElement
{
    public bool IsBatched { get { return this.ClassListContains(Constants.BLOCK_BATCH); } }
    public BlockDummyElement()
    {
        this.AddToClassList(Constants.BLOCK_NORMAL);
        this.pickingMode = PickingMode.Ignore;
    }

    public void BlockBatch(BlockInfo _blockInfo)
    {
        if (IsBatched) return;
        SetSprite(_blockInfo.BlockIMG);
        this.AddToClassList(Constants.BLOCK_BATCH);
    }
    public void DestroyBlock()
    {
        if (!IsBatched) return;
        this.RemoveFromClassList(Constants.BLOCK_BATCH);
    }
    public void SetSprite(string _spriteName)
    {
        this.style.backgroundImage = new StyleBackground(AtlasManager.Instance.GetSprite(_spriteName));
    }
}