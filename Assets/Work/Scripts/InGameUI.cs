using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;
using Utility;


public class InGameUI : MonoBehaviour
{

    private UIDocument doc;

    #region  Elements
    private VisualElement rootElement; // 최상단 VisualElement
    private VisualElement boardElement;
    private VisualElement bottomRoot;
    private SlotElement[] blockPickSlots;
    private TileElement highlightTile = null; // 가장 최근 마우스가 들어온 타일
    private TileElement[,] tiles; // 보드 타일맵
    private SlotElement selectedSlot; // 선택된 블록

    #endregion

    #region Debug Tools
    private List<VisualElement> debugs = new List<VisualElement>();
    #endregion

    private ElementsPool elementsPool;
    private List<Tile> destroyTargets;

    public static event Action OnGameOver;

    public void Init()
    {
        BindVariable();
        elementsPool = new ElementsPool();
        elementsPool.AddPool<BlockDummyElement>(Constants.BLOCK_KEY, 200, rootElement);


        StartCoroutine(NextFrame(InitGame));
    }
    private void OnEnable()
    {
        BlockDummyElement.OnDestroyedBlock += OnDestroyedBlock;
    }
    private void OnDisable()
    {
        BlockDummyElement.OnDestroyedBlock -= OnDestroyedBlock;
    }
    private void OnDestroyedBlock(BlockDummyElement _destroyedBlock)
    => elementsPool.ReturnParts<BlockDummyElement>(Constants.BLOCK_KEY, _destroyedBlock);

    private void BindVariable()
    {

        doc = GetComponent<UIDocument>();
        rootElement = doc.rootVisualElement;
        rootElement.RegisterCallback<MouseMoveEvent>(OnPickSlotMouseMove);
        rootElement.RegisterCallback<MouseUpEvent>(OnPickSlotMouseUp);
        boardElement = rootElement.Q("board");
        bottomRoot = rootElement.Q("bottom");

        destroyTargets = new List<Tile>();
        InitBottomBlockSlots();
    }
    private void InitBottomBlockSlots()
    {
        blockPickSlots = new SlotElement[bottomRoot.childCount];

        for (int i = 0; i < Constants.BLOCK_PICKSLOT_COUNT; i++)
        {
            SlotElement _newPickSlot = new SlotElement();
            _newPickSlot.AddToClassList(Constants.BLOCK_PICK);

            _newPickSlot.ChangeBlockInfo(TableManager.Instance.GetRandomBlockInfo());
            _newPickSlot.RegisterCallback<MouseDownEvent>(OnPickSlotMouseDown);

            bottomRoot.Add(_newPickSlot);
            blockPickSlots[i] = _newPickSlot;
        }
    }

    private void InitGame()
    {
        for (int i = 0; i < blockPickSlots.Length; i++)
        {
            BlockBuild(blockPickSlots[i], TableManager.Instance.GetRandomBlockInfo());
        }
        CreateBoard(8);
    }
    private void BlocksBatch(TileElement _tile, BlockInfo _blockInfo)
    {
        for (int i = 0; i < _blockInfo.BlockCoord.Length; i++)
        {
            BlockDummyElement _batchBlock = elementsPool.GetParts<BlockDummyElement>(Constants.BLOCK_KEY);
            int _nextX = _tile.InTile.TileCoord.x + _blockInfo.BlockCoord[i].x;
            int _nextY = _tile.InTile.TileCoord.y + _blockInfo.BlockCoord[i].y;

            TileElement _inTile = tiles[_nextY, _nextX];
            rootElement.Add(_batchBlock);

            Vector2 _newPos = _inTile.GetCenterPosition();//_tile.GetCenterPosition() + _blockInfo.BlockCoord[i] * _tile.layout.size;
            _batchBlock.style.width = _tile.layout.width;
            _batchBlock.style.height = _tile.layout.height;
            _batchBlock.Translate(_newPos);

            tiles[_nextY, _nextX].InTile.SetBlock(_batchBlock, _blockInfo);
        }
    }
    private bool CheckDestroyBlock()
    {
        if (destroyTargets == null) destroyTargets = new List<Tile>();

        destroyTargets.Clear();

        for (int y = 0; y < tiles.GetLength(0); y++)
        {
            bool _isHorizontal = true;
            bool _isVertical = true;

            for (int x = 0; x < tiles.GetLength(1); x++)
            {
                _isHorizontal &= tiles[y, x].InTile.IsBatched;
                _isVertical &= tiles[x, y].InTile.IsBatched;
            }

            if (_isHorizontal) AddHorizontalBlocks(y, destroyTargets);
            if (_isVertical) AddVerticalBlocks(y, destroyTargets);
        }

        return destroyTargets.Count > 0;
    }
    private void DestroyTargetBlocks()
    {
        if (destroyTargets == null) return;
        if (destroyTargets.Count == 0) return;

        for (int i = 0; i < destroyTargets.Count; i++)
            destroyTargets[i].RemoveBlock();
    }
    private void AddHorizontalBlocks(int _y, List<Tile> _blockList)
    {
        if (_blockList == null) throw new NullReferenceException("BlockList가 Null 입니다 : 매게변수 값 확인 필요");
        int _length = tiles.GetLength(0);

        for (int x = 0; x < _length; x++)
        {
            Tile _target = tiles[_y, x].InTile;
            if (_blockList.Contains(_target)) continue;
            _blockList.Add(_target);
        }
    }
    private void AddVerticalBlocks(int _x, List<Tile> _blockList)
    {
        if (_blockList == null) throw new NullReferenceException("BlockList가 Null 입니다 : 매게변수 값 확인 필요");

        int _length = tiles.GetLength(0);

        for (int y = 0; y < _length; y++)
        {
            Tile _target = tiles[y, _x].InTile;
            if (_blockList.Contains(_target)) continue;
            _blockList.Add(_target);
        }
    }
    private void BlockBuild(SlotElement _blockElement, BlockInfo _blockInfo)
    {
        _blockElement.ReturnPoolChildenElements(elementsPool);
        _blockElement.ChangeBlockInfo(_blockInfo);

        Vector2 _centerPos = _blockElement.GetLocalPosition();
        //Vector2 _blockCount = _blockInfo.BlockSize;// Constants.SLOT_SIZE; //_blockElement.InBlockInfo.GetSize();

        for (int i = 0; i < _blockElement.BatchCoord.Length; i++)
        {
            BlockDummyElement _block = elementsPool.GetParts<BlockDummyElement>(Constants.BLOCK_KEY);
            Vector2 _blockSize = new Vector2(_blockElement.layout.width / _blockInfo.BlockSize.x, _blockElement.layout.height / _blockInfo.BlockSize.y);

            _blockElement.childElement.Add(_block);
            _block.style.width = _blockSize.x;
            _block.style.height = _blockSize.y;


            float _nextX = _blockElement.BatchCoord[i].x * _blockSize.x;
            float _nextY = _blockElement.BatchCoord[i].y * _blockSize.y;

            Vector2 _nextPos = new Vector2(_nextX, _nextY);
            Vector2 _calculatedPos = _centerPos + new Vector2(_nextX, _nextY);

            _block.style.left = _calculatedPos.x;
            _block.style.top = _calculatedPos.y > 0 ? _centerPos.y - _nextPos.y : _centerPos.y + _nextPos.y;
            _block.style.backgroundImage = new StyleBackground(AtlasManager.Instance.GetSprite(_blockInfo.BlockIMG));
            _block.BlockBatch(_blockInfo);
        }
    }
    private void BlockBuild(VisualElement _center, SlotElement _blockElement)
    {
        Vector2 _centerPos = _center.GetLocalPosition();
        Vector2 _blockSize = _blockElement.InBlockInfo.GetSize();

        for (int i = 0; i < _blockElement.BatchCoord.Length; i++)
        {
            BlockDummyElement _block = elementsPool.GetParts<BlockDummyElement>(Constants.BLOCK_KEY);
            Vector2 _calculateBlockSize = new Vector2(_center.layout.width / _blockSize.x, _center.layout.height / _blockSize.y);

            _center.Add(_block);
            _block.style.width = _calculateBlockSize.x;
            _block.style.height = _calculateBlockSize.y;

            float _nextX = _blockElement.BatchCoord[i].x * _calculateBlockSize.x;
            float _nextY = _blockElement.BatchCoord[i].y * _calculateBlockSize.y;
            Vector2 _calculatedPos = _centerPos + new Vector2(_nextX, _nextY);

            _block.style.left = _calculatedPos.x;
            _block.style.top = _calculatedPos.y;
            _block.BlockBatch(TableManager.Instance.BlockInfos[0]);
        }
    }
    private bool IsGameOver()
    {
        for (int i = 0; i < blockPickSlots.Length; i++)
            if (IsCanBatch(blockPickSlots[i].InBlockInfo)) return false;

        return true;
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
    private SlotElement CreateBlockElement(string _defaultClass, BlockInfo _blockInfo)
    {
        SlotElement _newBlockElement = new SlotElement(_defaultClass, _blockInfo);
        return _newBlockElement;
    }
    private void RegisterEventTile(TileElement _newElement)
    {
        _newElement.RegisterCallback<MouseEnterEvent, TileElement>(OnMouseEnterSlot, _newElement);
        _newElement.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
    }

    private void OnMouseEnterSlot(MouseEnterEvent _event, TileElement _slot)
    {
        if (_slot == null) return;
        if (selectedSlot == null) return;
        if (IsOutOfBoard(_slot, selectedSlot)) return;
        if (!IsCanBatch(_slot, selectedSlot.InBlockInfo)) return;

        CheckHighLightBlocks(_slot);
        BatchBlock(_slot);

        _event.StopPropagation();
    }
    private void OnMouseLeave(MouseLeaveEvent _event)
    {
        DisableDebugObjs();
    }

    // 타일 기준으로 블록을 배치 가능한지 체크
    private bool IsCanBatch(TileElement _tile, BlockInfo _batchBlock)
    {
        if (_tile == null) return false;

        Coord _tileCoord = _tile.InTile.TileCoord;

        for (int i = 0; i < _batchBlock.BlockCoord.Length; i++)
        {
            int _nextX = _tileCoord.x + _batchBlock.BlockCoord[i].x;
            int _nextY = _tileCoord.y + _batchBlock.BlockCoord[i].y;

            if (IsOutOfBoard(_nextX, _nextY)) return false;
            if (tiles[_nextY, _nextX].InTile.IsBatched) return false;
        }

        return true;
    }
    // 전체 타일을 검사하여 블록이 배치가 가능한지 체크
    private bool IsCanBatch(BlockInfo _blockInfo)
    {
        for (int y = 0; y < tiles.GetLength(0); y++)
        {
            for (int x = 0; x < tiles.GetLength(1); x++)
            {
                TileElement _currTile = tiles[y, x];
                if (_currTile.InTile.IsBatched) continue;
                if (IsCanBatch(_currTile, _blockInfo)) return true;
            }
        }

        return false;
    }
    private bool IsOutOfBoard(TileElement _centerTile, SlotElement _block)
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
    private void GameOver()
    {
        for (int i = 0; i < blockPickSlots.Length; i++)
            blockPickSlots[i].UnregisterCallback<MouseDownEvent>(OnPickSlotMouseDown);
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

    private void BatchBlock(TileElement _tile)
    {

        Vector2 _tilePos = _tile.GetCenterPosition();


        for (int i = 0; i < selectedSlot.BatchCoord.Length; i++)
        {
            Coord _centerCoord = _tile.InTile.TileCoord;
            Coord _currentCoord = selectedSlot.BatchCoord[i];
            int _nextX = _centerCoord.x + _currentCoord.x;
            int _nextY = _centerCoord.y + _currentCoord.y;

            if (IsOutOfBoard(_nextX, _nextY)) return;

            Vector2 _elementPos = tiles[_nextY, _nextX].GetCenterPosition();
            GetDebugObj().Translate(_elementPos);
        }
        GetDebugObj().Translate(_tilePos);
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
        if (selectedSlot != null)
        {
            // FIXME: 현재 선택된 블록 제자리 이동 및 새로운 블록 선택 기능 작성
        }
        else
        {
            selectedSlot = _evt.target as SlotElement;
            selectedSlot.DragStart(_evt);
            rootElement.Add(selectedSlot.childElement);
        }
    }
    private void OnPickSlotMouseUp(MouseUpEvent _evt)
    {
        if (selectedSlot == null) return;

        TileElement _tileElement = _evt.target as TileElement;
        bool _isBatchSuccess = IsCanBatch(_tileElement, selectedSlot.InBlockInfo);

        if (_isBatchSuccess)
        {
            BlocksBatch(_tileElement, selectedSlot.InBlockInfo);
            BlockBuild(selectedSlot, TableManager.Instance.GetRandomBlockInfo());

            bool _isNeedDestroy = CheckDestroyBlock();

            if (_isNeedDestroy)
                DestroyTargetBlocks();

            if (IsGameOver())
            {
                GameOver();
                OnGameOver?.Invoke();
            }

        }

        selectedSlot.DragEnd();
        selectedSlot = null;

        _evt.StopPropagation();
    }
    private void OnPickSlotMouseMove(MouseMoveEvent _evt)
    {
        if (selectedSlot == null) return;

        selectedSlot.Dragging(_evt);
        _evt.StopPropagation();
    }
    #endregion
}
