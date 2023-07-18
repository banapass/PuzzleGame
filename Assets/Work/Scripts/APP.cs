using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APP : Singleton<APP>
{
    protected override void Awake()
    {
        base.Awake();

    }
    private void Start()
    {
        InitManagers();
    }
    private void InitManagers()
    {
        TableManager.Instance.Init();
        UIManager.Instance.Init();
    }
}
