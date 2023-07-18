using UnityEngine.UIElements;

public class TileElement : VisualElement
{
    public Tile InTile { get; private set; }
    public Coord[] BatchCoord { get; private set; }
    public TileElement(string _className, Tile _targetTile)
    {
        this.AddToClassList(_className);
        InTile = _targetTile;
        BatchCoord = Coord.FourDirection;
    }
}

public class BlockElement : VisualElement, IObjectable
{
    public BlockInfo InBlockInfo { get; private set; }
    public Coord[] BatchCoord { get { return InBlockInfo.BlockCoord; } }

    public string ObjectID { get; set; }

    public BlockElement()
    {

    }
    public BlockElement(string _className, BlockInfo _blockInfo)
    {
        if (_className != null) this.AddToClassList(_className);
        InBlockInfo = _blockInfo;
    }
}