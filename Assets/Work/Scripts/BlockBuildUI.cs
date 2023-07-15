using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
#if UNITY_EDITOR
public class BlockBuildUI : MonoBehaviour
{
    [SerializeField] BlockCreateEditor blockEditor;
    VisualElement root;
    public static Action<MouseEnterEvent> OnMouseEnterBtn;
    public static Action<MouseLeaveEvent> OnMouseLeaveBtn;
    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        Button _saveBtn = root.Q<Button>("save_btn");
        Button _loadBtn = root.Q<Button>("load_btn");
        Button[] _btns = new Button[] { _saveBtn, _loadBtn };

        _saveBtn.RegisterCallback<ClickEvent>(_event => blockEditor.SavePrefab());
        _loadBtn.RegisterCallback<ClickEvent>(_event => blockEditor.LoadPrefab());

        for (int i = 0; i < _btns.Length; i++)
        {
            _btns[i].RegisterCallback<MouseEnterEvent>(_event => { OnMouseEnterBtn?.Invoke(_event); });
            _btns[i].RegisterCallback<MouseLeaveEvent>(_event => { OnMouseLeaveBtn?.Invoke(_event); });
        }
    }
}
#endif