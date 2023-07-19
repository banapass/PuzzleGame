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
    private VisualElement[] blockPickSlots;
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
    private void DebugInit()
    {
        // 디버깅 용
        selectedBlock = new BlockElement("block", TableManager.Instance.BlockInfos[0]);
        rootElement.Add(selectedBlock.CloneVisualElement<BlockElement>());
        // rootElement.Add();
        //DisableDebugObjs();
        // BlockBuild();
        // 디버깅 용
        for (int i = 0; i < blockPickSlots.Length; i++)
        {
            BlockBuild(blockPickSlots[i], selectedBlock);
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
    private void BlockBuild(VisualElement _center, BlockElement _blockElement)
    {
        Vector2 _centerPos = _center.GetLocalPosition();
        Vector2 _blockCount = _blockElement.InBlockInfo.GetSize();
        //_blockElement.InBlockInfo.GetSize();

        for (int i = 0; i < _blockElement.BatchCoord.Length; i++)
        {
            BlockDummyElement _block = elementsPool.GetParts<BlockDummyElement>(Constants.BLOCK_KEY);
            Vector2 _blockSize = new Vector2(_center.layout.width / _blockCount.x, _center.layout.height / _blockCount.y);

            _center.Add(_block);
            _block.style.width = _blockSize.x;//_center.layout.width / _blockSize.x;//new StyleLength(new Length(, LengthUnit.Percent));
            _block.style.height = _blockSize.y;//_center.layout.height / _blockSize.y;//new StyleLength(new Length(, LengthUnit.Percent));


            if (float.IsNaN(_block.layout.width))
            {
                StartCoroutine(NextFrame<int>(_index =>
                {
                    float _nextX = _blockElement.BatchCoord[_index].x * _blockSize.x;// _block.layout.width;
                    float _nextY = _blockElement.BatchCoord[_index].y * _blockSize.y;//_block.layout.height;
                    Vector2 _calculatedPos = _centerPos + new Vector2(_nextX, _nextY);

                    _block.style.left = _calculatedPos.x;
                    _block.style.top = _calculatedPos.y;
                    _block.BlockBatch(TableManager.Instance.BlockInfos[0]);
                }, i));
            }
            else
            {
                Debug.Log("IN Un Nan");
                float _nextX = _blockElement.BatchCoord[i].x * _blockSize.x;
                float _nextY = _blockElement.BatchCoord[i].y * _blockSize.y;
                Vector2 _calculatedPos = _centerPos + new Vector2(_nextX, _nextY);

                _block.style.left = _calculatedPos.x;
                _block.style.top = _calculatedPos.y;
                _block.BlockBatch(TableManager.Instance.BlockInfos[0]);
            }
        }
    }
    private void BindVariable()
    {
        doc = GetComponent<UIDocument>();
        rootElement = doc.rootVisualElement;
        boardElement = rootElement.Q("board");
        bottomRoot = rootElement.Q("bottom");
        blockPickSlots = new VisualElement[bottomRoot.childCount];

        // boardElement.RegisterCallback<GeometryChangedEvent, VisualElement>(ResizeElement, boardElement);

        int _count = 0;
        foreach (var _child in bottomRoot.Children())
        {
            blockPickSlots[_count] = _child;
            _count++;
        }
        // Debug.Log(blockPickSlots.Length);
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

                _newSlot.RegisterCallback<MouseEnterEvent, TileElement>(OnMouseEnterSlot, _newSlot);
                _newSlot.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
                _newSlot.RegisterCallback<TransitionStartEvent, string>(OnStartEvent, "Destroy");
                _newSlot.RegisterCallback<PointerDownEvent>(_event =>
                {
                    Debug.Log("PointerDown");
                });
                _newSlot.RegisterCallback<DragEnterEvent>(_event =>
                {
                    Debug.Log("Drag");
                });
                _newSlot.focusable = true;

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
    private BlockElement CreateBlockElement(string _defaultClass, BlockInfo _blockInfo)
    {
        BlockElement _newBlockElement = new BlockElement(_defaultClass, _blockInfo);
        return _newBlockElement;
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

}
