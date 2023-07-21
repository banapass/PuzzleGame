using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

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
    public VisualElement childElement;

    public BlockElement()
    {
        SetChildElement();
    }
    public BlockElement(string _className, BlockInfo _blockInfo)
    {
        if (_className != null) this.AddToClassList(_className);
        InBlockInfo = _blockInfo;
        SetChildElement();
    }

    public void ChangeBlockInfo(BlockInfo _blockInfo)
    {
        InBlockInfo = _blockInfo;
    }
    private void SetChildElement()
    {
        if (childElement != null) return;

        childElement = new VisualElement();
        childElement.pickingMode = PickingMode.Ignore;
        this.Add(childElement);
    }
    public void DragStart(MouseDownEvent _evt)
    {
        MoveToMousePosition<MouseDownEvent>(_evt);
        childElement.style.position = new StyleEnum<Position>(Position.Absolute);
    }
    public void Dragging(MouseMoveEvent _evt)
    {
        MoveToMousePosition<MouseMoveEvent>(_evt);
    }
    public void DragEnd()
    {
        if (this.Contains(childElement)) return;
        ResetChildPosition();
        this.Add(childElement);
    }
    private void ResetChildPosition()
    {
        childElement.style.position = new StyleEnum<Position>(Position.Relative);
        childElement.style.left = 0;
        childElement.style.top = 0;
    }
    public void MoveToMousePosition<T>(T _evt) where T : IMouseEvent
    {
        Vector2 _newPosition = _evt.mousePosition - Constants.DRAG_OFFSET;
        childElement.style.left = _newPosition.x;
        childElement.style.top = _newPosition.y;
    }
}

public class BlockDummyElement : VisualElement
{
    //public static event Action<string, VisualElement> OnDestryed;
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
        //this.AddToClassList(Constants.BLOCK_BATCH);
        style.scale = Vector2.zero;
        DOTween.To(() => style.scale.value.value, x => style.scale = x, Vector2.one, 0.5f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                Debug.Log("ON Completed");
            });
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