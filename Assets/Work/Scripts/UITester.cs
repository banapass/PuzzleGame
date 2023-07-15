using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;


public class TileVisualElement : VisualElement
{
    public Tile InTile { get; private set; }
    public TileVisualElement(string _className, Tile _targetTile)
    {
        this.AddToClassList(_className);
        InTile = _targetTile;
    }
}

public class UITester : MonoBehaviour
{

    private UIDocument doc;
    private VisualElement rootElement;
    private VisualElement debug;
    [SerializeField] Transform target;
    private Camera mainCamera;
    private Tile[,] tiles;
    private List<VisualElement> debugs = new List<VisualElement>();



    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        rootElement = doc.rootVisualElement;

        debug = rootElement.Q("obj");
        VisualElement _block = rootElement.Q("block");

        int _size = 5;
        // float _createCount = Mathf.Pow(_size, 2);
        StyleLength _newSlotSize = new StyleLength(new Length(100 / _size, LengthUnit.Percent));
        tiles = new Tile[_size, _size];

        for (int y = 0; y < _size; y++)
        {
            for (int x = 0; x < _size; x++)
            {
                Tile _newTile = CreateNewTile(x, y);
                tiles[y, x] = _newTile;

                TileVisualElement _newSlot = new TileVisualElement("block-slot", _newTile);

                _newSlot.RegisterCallback<MouseEnterEvent, TileVisualElement>(OnMouseEnterSlot, _newSlot);
                _newSlot.RegisterCallback<TransitionStartEvent, string>(OnStartEvent, "Destroy");

                _newSlot.style.width = _newSlotSize;
                _newSlot.style.height = _newSlotSize;

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
    private void OnMouseEnterSlot(MouseEnterEvent _event, TileVisualElement _slot)
    {
        if (_slot == null) return;

        Vector2 _slotPos = _slot.GetCenterPosition();
        debug.style.left = _slotPos.x;
        debug.style.top = _slotPos.y;

        _event.StopPropagation();
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
