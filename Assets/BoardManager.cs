using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer tilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        CreateBoard(4);
    }

    private void CreateBoard(int _size)
    {
        bool _isHolsu = _size % 2 != 0;
        int _start = 0;

        if (_isHolsu)
            _start = _size - 2;
        else
            _start = _size / 2;

        Vector2 _startPos = new Vector2(-_start, -_start);
        float _count = 0;
        float _maxColorCount = Mathf.Pow(_size, 2);
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                var _tile = Instantiate(tilePrefab);
                Vector2 _tileSize = _tile.size;
                Vector2 _nextPos = new Vector2(x, y) * _tileSize;
                if (!_isHolsu)
                    _nextPos += Vector2.one * 0.5f;
                Debug.Log($"Count : {_count} / MaxCount : {_maxColorCount} / Amount : {_count / _maxColorCount}");
                _tile.color = Color.Lerp(Color.white, Color.black, _count / _maxColorCount);
                _tile.transform.position = _startPos + _nextPos;
                _count++;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector2.one);
    }
}
