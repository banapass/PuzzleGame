using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    private UIDocument doc;
    public void Init()
    {
        doc = GetComponent<UIDocument>();
        gameObject.AddComponent<InGameUI>().Init();
    }
}
