using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UITester : MonoBehaviour
{

    private UIDocument doc;
    private VisualElement rootElement;
    [SerializeField] Transform target;
    private Camera mainCamera;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        doc.panelSettings.scale = CalculateScaleFromResolution(1080, 1920);
        rootElement = doc.rootVisualElement;

        rootElement.RegisterCallback<MouseEnterEvent>(_event =>
        {
            if (mainCamera == null) mainCamera = Camera.main;

            Debug.Log(rootElement.resolvedStyle.position);
            Debug.Log(rootElement.transform);
            target.transform.position = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        });




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
