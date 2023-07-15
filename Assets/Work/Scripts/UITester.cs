using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

public class UITester : MonoBehaviour
{

    private UIDocument doc;
    private VisualElement rootElement;
    [SerializeField] Transform target;
    private Camera mainCamera;



    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        // doc.panelSettings.scale = CalculateScaleFromResolution(1080, 1920);
        rootElement = doc.rootVisualElement;
        // VisualElement _debug = rootElement.Q("debug");
        VisualElement _obj = rootElement.Q("obj");
        VisualElement _block = rootElement.Q("block");
        // _debug.transform.position = _block.transform.position;

        // Debug.Log(_debug.transform.position);
        Debug.Log(_block.transform.position);


        int _size = 5;
        float _createCount = Mathf.Pow(_size, 2);
        StyleLength _newSlotSize = new StyleLength(new Length(100 / _size, LengthUnit.Percent));

        for (int i = 0; i < _createCount; i++)
        {
            VisualElement _newSlot = new VisualElement();
            _newSlot.AddToClassList("block-slot");
            _newSlot.style.width = _newSlotSize;
            _newSlot.style.height = _newSlotSize;

            _block.Add(_newSlot);
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
    private void CreateBoardSlots(int _size)
    {

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
