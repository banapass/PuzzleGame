using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;


public class InGameUI : MonoBehaviour
{

    private UIDocument doc;

    #region  Elements
    private VisualElement rootElement; // 최상단 VisualElement
    private VisualElement boardElement;
    private VisualElement bottomRoot;
    private BlockElement[] blockPickSlots;
    private TileElement highlightTile = null; // 가장 최근 마우스가 들어온 타일
    private TileElement[,] tiles; // 보드 타일맵
    private BlockElement selectedBlock; // 선택된 블록

    #endregion

    #region Debug Tools
    private List<VisualElement> debugs = new List<VisualElement>();
    #endregion

    private ElementsPool elementsPool;

    public void Init()
    {
        //for (int i = 0; i < 20; i++)
        //    CreateDebugElement();
        BindVariable();
        elementsPool = new ElementsPool();
        elementsPool.AddPool<BlockDummyElement>(Constants.BLOCK_KEY, 60, rootElement);

        StartCoroutine(NextFrame(DebugInit));

    }
    private void BindVariable()
    {
        doc = GetComponent<UIDocument>();
        rootElement = doc.rootVisualElement;
        rootElement.RegisterCallback<MouseMoveEvent>(OnPickSlotMouseMove);
        rootElement.RegisterCallback<MouseUpEvent>(OnPickSlotMouseUp);
        boardElement = rootElement.Q("board");
        bottomRoot = rootElement.Q("bottom");
        InitBottomBlockSlots();
        // Debug.Log(blockPickSlots.Length);
    }
    private void InitBottomBlockSlots()
    {
        blockPickSlots = new BlockElement[bottomRoot.childCount];

        for (int i = 0; i < Constants.BLOCK_PICKSLOT_COUNT; i++)
        {
            BlockElement _newPickSlot = new BlockElement();
            _newPickSlot.AddToClassList(Constants.BLOCK_PICK);

            _newPickSlot.ChangeBlockInfo(TableManager.Instance.BlockInfos[0]);
            _newPickSlot.RegisterCallback<MouseDownEvent>(OnPickSlotMouseDown);

            bottomRoot.Add(_newPickSlot);
            blockPickSlots[i] = _newPickSlot;
        }
    }

    private void DebugInit()
    {
        // selectedBlock = new BlockElement("block", TableManager.Instance.BlockInfos[0]);
        // selectedBlock.CloneVisualElement<BlockElement>()
        // rootElement.Add();

        for (int i = 0; i < blockPickSlots.Length; i++)
        {
            BlockBuild(blockPickSlots[i]);
        }
        CreateBoard(5);
    }
    private void BlockBuild()
    {
        Vector2 _centerPos = boardElement.GetCenterPosition();
        for (int i = 0; i < selectedBlock.BatchCoord.Length; i++)
        {
            VisualElement _block = GetDebugObj();

            float _nextX = selectedBlock.BatchCoord[i].x * _block.layout.width;
            float _nextY = selectedBlock.BatchCoord[i].y * _block.layout.height;
            Vector2 _calculatedPos = _centerPos + new Vector2(_nextX, _nextY);

            _block.style.left = _calculatedPos.x;
            _block.style.top = _calculatedPos.y;
            _block.style.backgroundImage = new StyleBackground(AtlasManager.Instance.GetSprite("redBlock 1"));

        }
    }
    private void BlockBuild(BlockElement _blockElement)
    {
        Vector2 _centerPos = _blockElement.GetLocalPosition();
        Vector2 _blockCount = _blockElement.InBlockInfo.GetSize();

        for (int i = 0; i < _blockElement.BatchCoord.Length; i++)
        {
            BlockDummyElement _block = elementsPool.GetParts<BlockDummyElement>(Constants.BLOCK_KEY);
            Vector2 _blockSize = new Vector2(_blockElement.layout.width / _blockCount.x, _blockElement.layout.height / _blockCount.y);

            _blockElement.childElement.Add(_block);
            _block.style.width = _blockSize.x;
            _block.style.height = _blockSize.y;

            float _nextX = _blockElement.BatchCoord[i].x * _blockSize.x;
            float _nextY = _blockElement.BatchCoord[i].y * _blockSize.y;
            Vector2 _calculatedPos = _centerPos + new Vector2(_nextX, _nextY);

            _block.style.left = _calculatedPos.x;
            _block.style.top = _calculatedPos.y;
            _block.BlockBatch(TableManager.Instance.BlockInfos[0]);
        }
    }
    private void BlockBuild(VisualElement _center, BlockElement _blockElement)
    {
        Vector2 _centerPos = _center.GetLocalPosition();
        Vector2 _blockCount = _blockElement.InBlockInfo.GetSize();

        for (int i = 0; i < _blockElement.BatchCoord.Length; i++)
        {
            BlockDummyElement _block = elementsPool.GetParts<BlockDummyElement>(Constants.BLOCK_KEY);
            Vector2 _blockSize = new Vector2(_center.layout.width / _blockCount.x, _center.layout.height / _blockCount.y);

            _center.Add(_block);
            _block.style.width = _blockSize.x;
            _block.style.height = _blockSize.y;

            float _nextX = _blockElement.BatchCoord[i].x * _blockSize.x;
            float _nextY = _blockElement.BatchCoord[i].y * _blockSize.y;
            Vector2 _calculatedPos = _centerPos + new Vector2(_nextX, _nextY);

            _block.style.left = _calculatedPos.x;
            _block.style.top = _calculatedPos.y;
            _block.BlockBatch(TableManager.Instance.BlockInfos[0]);

            // if (float.IsNaN(_block.layout.width))
            // {
            //     StartCoroutine(NextFrame<int>(_index =>
            //     {
            //         float _nextX = _blockElement.BatchCoord[_index].x * _blockSize.x;
            //         float _nextY = _blockElement.BatchCoord[_index].y * _blockSize.y;
            //         Vector2 _calculatedPos = _centerPos + new Vector2(_nextX, _nextY);

            //         _block.style.left = _calculatedPos.x;
            //         _block.style.top = _calculatedPos.y;
            //         _block.BlockBatch(TableManager.Instance.BlockInfos[0]);
            //     }, i));
            // }
            // else
            // {

            // }
        }
    }


    private void CreateBoard(int _size)
    {
        StyleLength _newSlotSize = new StyleLength(new Length(100 / _size, LengthUnit.Percent));
        tiles = new TileElement[_size, _size];

        for (int y = 0; y < _size; y++)
        {
            for (int x = 0; x < _size; x++)
            {
                Tile _newTile = CreateNewTile(x, y);

                TileElement _newSlot = new TileElement("block-slot", _newTile);

                RegisterEventTile(_newSlot);

                _newSlot.style.width = _newSlotSize;
                _newSlot.style.height = _newSlotSize;

                tiles[y, x] = _newSlot;
                boardElement.Add(_newSlot);
            }
        }
    }
    private Tile CreateNewTile(int x, int y)
    {
        Tile _newTile = new Tile();
        _newTile.SetCoord(x, y);

        return _newTile;
    }
    private BlockElement CreateBlockElement(string _defaultClass, BlockInfo _blockInfo)
    {
        BlockElement _newBlockElement = new BlockElement(_defaultClass, _blockInfo);
        return _newBlockElement;
    }
    private void RegisterEventTile(TileElement _newElement)
    {
        _newElement.RegisterCallback<MouseEnterEvent, TileElement>(OnMouseEnterSlot, _newElement);
        _newElement.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        _newElement.RegisterCallback<TransitionStartEvent, string>(OnStartEvent, "Destroy");
    }

    private void OnStartEvent(TransitionStartEvent evt, string _className)
    {

    }

    private void CreateBoardSlots(int _size)
    {

    }
    private void OnMouseEnterSlot(MouseEnterEvent _event, TileElement _slot)
    {
        if (_slot == null) return;
        if (selectedBlock == null) return;
        if (IsOutOfBoard(_slot, selectedBlock)) return;

        // Vector2 _slotPos = _slot.GetCenterPosition();
        // debug.style.left = _slotPos.x;
        // debug.style.top = _slotPos.y;
        CheckHighLightBlocks(_slot);
        BatchBlock(_slot);

        _event.StopPropagation();
    }
    private void OnMouseLeave(MouseLeaveEvent _event)
    {
        DisableDebugObjs();
    }
    private void BatchBlock(TileElement _tile)
    {

        Vector2 _tilePos = _tile.GetCenterPosition();


        for (int i = 0; i < selectedBlock.BatchCoord.Length; i++)
        {
            Coord _centerCoord = _tile.InTile.TileCoord;
            Coord _currentCoord = selectedBlock.BatchCoord[i];
            int _nextX = _centerCoord.x + _currentCoord.x;
            int _nextY = _centerCoord.y + _currentCoord.y;

            if (IsOutOfBoard(_nextX, _nextY)) return;

            Vector2 _elementPos = tiles[_nextY, _nextX].GetCenterPosition();
            GetDebugObj().Translate(_elementPos);
        }
        GetDebugObj().Translate(_tilePos);
        // _block.style.left = _tilePos.x;
        // _block.style.top = _tilePos.y;
    }
    private bool IsOutOfBoard(TileElement _centerTile, BlockElement _block)
    {
        for (int i = 0; i < _block.BatchCoord.Length; i++)
        {
            Coord _centerCoord = _centerTile.InTile.TileCoord;
            Coord _currBlockCoord = _block.BatchCoord[i];
            int _nextX = _centerCoord.x + _currBlockCoord.x;
            int _nextY = _centerCoord.y + _currBlockCoord.y;
            if (IsOutOfBoard(_nextX, _nextY)) return true;
        }
        return false;


    }
    private bool IsOutOfBoard(int _x, int _y)
    {
        if (_x < 0 || _y < 0) return true;
        if (tiles.GetLength(0) - 1 < _x) return true;
        if (tiles.GetLength(1) - 1 < _y) return true;

        return false;
    }
    private void CheckHighLightBlocks(TileElement _nextTile)
    {
        if (highlightTile == _nextTile) return;

        if (highlightTile == null)
        {
            highlightTile = _nextTile;
            return;
        }
        else
        {
            DisableDebugObjs();
        }

    }
    private void ResizeElement(GeometryChangedEvent evt, VisualElement targetElement)
    {
        // float targetWidth = 720;
        // // float targetHeight = targetWidth / AspectRatio;
        // targetElement.style.width = targetWidth;
        // targetElement.style.height = targetWidth;
        Debug.Log(targetElement.layout);
    }


    #region Debug Func
    private void DisableDebugObjs()
    {
        for (int i = 0; i < debugs.Count; i++)
        {
            debugs[i].visible = false;
        }
    }
    private VisualElement GetDebugObj()
    {
        for (int i = 0; i < debugs.Count; i++)
        {
            if (debugs[i].visible) continue;
            debugs[i].visible = true;
            return debugs[i];
        }
        return CreateDebugElement();
    }
    private VisualElement CreateDebugElement()
    {
        VisualElement _newDebugObj = new VisualElement();
        _newDebugObj.AddToClassList("debug-block");
        _newDebugObj.pickingMode = PickingMode.Ignore;
        debugs.Add(_newDebugObj);
        rootElement.Add(_newDebugObj);
        // _newDebugObj.MarkDirtyRepaint();

        return _newDebugObj;
    }

    #endregion
    private IEnumerator NextFrame(Action _callBack)
    {
        yield return new WaitForEndOfFrame();
        _callBack();
    }
    private IEnumerator NextFrame<T>(Action<T> _callBack, T _param) where T : struct
    {
        yield return new WaitForEndOfFrame();

        _callBack(_param);
    }

    #region Event
    private void OnPickSlotMouseDown(MouseDownEvent _evt)
    {
        if (selectedBlock != null)
        {

        }
        else
        {
            selectedBlock = _evt.target as BlockElement;

            // VisualElement _dragSlot = selectedBlock.childElement;
            // Vector2 _newPos = _evt.mousePosition + Constants.DRAG_OFFSET;
            // _dragSlot.style.left = _newPos.x;
            // _dragSlot.style.top = _newPos.y;
            selectedBlock.DragStart(_evt);
            rootElement.Add(selectedBlock.childElement);
            BlockBuild(selectedBlock.childElement, selectedBlock);

        }
    }
    private void OnPickSlotMouseUp(MouseUpEvent _evt)
    {
        if (selectedBlock == null) return;
        if ((_evt.target as TileElement) != null)
        {
            Debug.Log("ININININI");
        }

        selectedBlock.DragEnd();
        selectedBlock = null;

        _evt.StopPropagation();

        // VisualElement _dragSlot = selectedBlock.childElement;
        // selectedBlock.Add(_dragSlot);
        // _dragSlot.style.position = new StyleEnum<Position>(Position.Relative);
        // _dragSlot.style.left = 0;
        // _dragSlot.style.top = 0;
    }
    private void OnPickSlotMouseMove(MouseMoveEvent _evt)
    {
        if (selectedBlock == null) return;

        selectedBlock.Dragging(_evt);
        _evt.StopPropagation();

        // VisualElement _dragSlot = selectedBlock.childElement;
        // Vector2 _newPos = _evt.mousePosition - Constants.DRAG_OFFSET;
        // _dragSlot.style.left = _newPos.x;
        // _dragSlot.style.top = _newPos.y;
    }
    #endregion
}
