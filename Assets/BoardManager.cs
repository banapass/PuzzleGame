using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [field: SerializeField]
    public Camera GameCamera { get; private set; }

    [SerializeField] SpriteRenderer tilePrefab;
    [SerializeField] SpriteRenderer gameSpace;
    [SerializeField] int createCount;
    // Start is called before the first frame update
    void Start()
    {
        CreateBoard(createCount);
        NewCameraResizer(createCount);
        // CameraSetting(4);
        // Debug.Log();
    }

    private void CreateBoard(int _size)
    {
        bool _isHolsu = _size % 2 != 0;
        float _start = 0;

        if (_isHolsu)
            _start = (int)(_size / 2);
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
                if (!_isHolsu) _nextPos += Vector2.one * 0.5f;

                // Debug.Log($"Count : {_count} / MaxCount : {_maxColorCount} / Amount : {_count / _maxColorCount}");
                _tile.color = Color.Lerp(Color.white, Color.black, _count / _maxColorCount);
                _tile.transform.position = _startPos + _nextPos;
                _count++;
            }
        }
    }
    private void CameraSetting(int _size)
    {
        CalculateOrthographicSize();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector2.one);
    }
    private void CalculateOrthographicSize()
    {
        Vector3 spriteSize = new Vector2(gameSpace.sprite.textureRect.width, gameSpace.sprite.textureRect.height);

        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        float targetAspect = spriteSize.x / spriteSize.y;
        float currentAspect = screenWidth / screenHeight;

        float orthoSize = GameCamera.orthographicSize;

        if (currentAspect < targetAspect)
        {
            // 화면의 가로 비율이 스프라이트 박스의 가로 비율보다 작을 경우, orthoSize를 조정하여 스프라이트 박스가 화면에 맞출 수 있도록 합니다.
            float differenceInSize = targetAspect / currentAspect;
            orthoSize *= differenceInSize;
        }

        GameCamera.orthographicSize = orthoSize;
    }
    private void NewCameraResizer(int _count)
    {
        Vector2 spriteSize = tilePrefab.bounds.size * (_count + 2);

        // SpriteRenderer의 가로 크기를 기준으로 orthographic size 계산
        float orthoSize = spriteSize.x * 0.5f / GameCamera.aspect;

        // 카메라의 orthographic size 설정
        GameCamera.orthographicSize = orthoSize;
    }
}
