using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class ElementsPool
{
    private Dictionary<string, Queue<VisualElement>> poolDict;
    public void AddPool(VisualElement _elemnet, string _key, int _poolingCount)
    {
        if (poolDict.ContainsKey(_key))
            Debug.LogError("이미 존재하는 풀링 ID 입니다.");

        Queue<VisualElement> _elementPool = new Queue<VisualElement>();
        for (int i = 0; i < _poolingCount; i++)
        {
            VisualElement _newElement = _elemnet.visualTreeAssetSource.CloneTree();
            _newElement.visible = false;

            _elementPool.Enqueue(_newElement);
        }
    }
    public void AddPool(string _className, string _key, int _poolingCount)
    {
        if (poolDict.ContainsKey(_key))
            Debug.LogError("이미 존재하는 풀링 ID 입니다.");

        Queue<VisualElement> _elementPool = new Queue<VisualElement>();
        for (int i = 0; i < _poolingCount; i++)
        {
            VisualElement _newElement = new VisualElement();
            _newElement.AddToClassList(_className);

            _elementPool.Enqueue(_newElement);
        }
    }

    // public VisualElement GetElement(string _key)
    // {
    //     if (!poolDict.ContainsKey(_key))
    //         return null;

    //     Queue<VisualElement> _pool = poolDict[_key];

    //     if (_pool.Count > 0)
    //     {
    //         VisualElement _element = _pool.Dequeue();
    //         _element.visible = true;
    //         return _element;
    //     }
    //     else
    //     {

    //     }

    // }
}