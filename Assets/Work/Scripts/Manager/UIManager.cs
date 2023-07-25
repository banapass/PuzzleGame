using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private UIDocument doc;
    [SerializeField] private UIDocument popupDoc;

    private VisualElement popupRoot;

    public void Init()
    {
        doc = GetComponent<UIDocument>();
        gameObject.AddComponent<InGameUI>().Init();
        popupRoot = popupDoc.rootVisualElement;
    }
    private void OnEnable()
    {
        InGameUI.OnGameOver += OnGameOver;
    }
    private void OnDisable()
    {
        InGameUI.OnGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        ShowPopup("popup");
    }
    public void ShowPopup(string _popupName)
    {
        VisualElement _popup = popupRoot.Q(_popupName);
        _popup.AddToClassList(UIPopup.POPUP_SHOW);
    }

}
