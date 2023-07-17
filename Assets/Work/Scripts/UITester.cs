using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;


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



public class UITester : MonoBehaviour
{

    private UIDocument doc;
    private VisualElement rootElement;
    private VisualElement debug;
    private TileElement highlightTile = null;
    [SerializeField] Transform target;
    private Camera mainCamera;
    private TileElement[,] tiles;
    private List<VisualElement> debugs = new List<VisualElement>();
    private BlockElement selectedBlock;


    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        rootElement = doc.rootVisualElement;

        // 디버깅 용
        // debug = rootElement.Q("obj");
        // debugs.Add(debug);
        for (int i = 0; i < 5; i++)
        {
            VisualElement _newDebugObj = new VisualElement();
            _newDebugObj.AddToClassList("debug-block");
            debugs.Add(_newDebugObj);
            rootElement.Add(_newDebugObj);
            _newDebugObj.pickingMode = PickingMode.Ignore;
        }
        selectedBlock = new BlockElement(null, TableManager.Instance.BlockInfos[0]);

        DisableDebugObjs();
        // 디버깅 용

        VisualElement _block = rootElement.Q("block");

        int _size = 5;
        // float _createCount = Mathf.Pow(_size, 2);
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

                _newSlot.style.width = _newSlotSize;
                _newSlot.style.height = _newSlotSize;


                tiles[y, x] = _newSlot;
                _block.Add(_newSlot);
            }
        }

        // _debug.RegisterCallback<MouseDownEvent>(_event =>
        // {
        //     Vector2 clickPosition = _event.mousePosition;

        //     // Debug.Log(_debug.layout.x);
        //     // Debug.Log(_debug.layout.y);
        //     // Debug.Log(_debug.layout);
        //     // VisualElement 이동하기
        //     // (_debug.worldTransform.GetPosition() + new Vector3(_debug.layout.width, _debug.layout.height) * 0.5f);
        //     Vector2 _newPos = _debug.GetCenterPosition();
        //     _obj.style.width = _debug.layout.width;
        //     _obj.style.height = _debug.layout.height;


        //     _obj.style.left = _newPos.x;
        //     _obj.style.top = _newPos.y;


        //     _event.StopPropagation();
        // });

        // rootElement.RegisterCallback<MouseEnterEvent>(_event =>
        // {
        //     if (mainCamera == null) mainCamera = Camera.main;

        //     Debug.Log(rootElement.resolvedStyle.position);
        //     Debug.Log(rootElement.transform);
        //     // target.transform.position = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // });




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
        return null;
    }
    public float CalculateScaleFromResolution(float referenceResolutionWidth, float referenceResolutionHeight)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float aspectRatio = screenWidth / screenHeight;
        float referenceAspectRatio = referenceResolutionWidth / referenceResolutionHeight;

        float scale = aspectRatio / referenceAspectRatio;

        return scale;
    }

}
