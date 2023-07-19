namespace PuzzleGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BoardManager : MonoBehaviour
    {
        [field: SerializeField]
        public Camera GameCamera { get; private set; }
        [SerializeField] Transform boardArea;
        // [SerializeField] Tile tilePrefab;
        [SerializeField] int createCount;
        [SerializeField] int cameraSpecing;

        // Start is called before the first frame update
        // void Start()
        // {
        //     CreateBoard(createCount);
        //     CameraResizing(boardArea.localScale.x);
        // }

        // private void CreateBoard(int _size)
        // {
        //     bool _isOddNumber = _size % 2 != 0;
        //     float _start = _size / 2;


        //     Vector2 _resolusionSize = boardArea.localScale / createCount;
        //     Vector2 _startPos = (Vector2)boardArea.transform.position + new Vector2(-_start, -_start) * _resolusionSize;

        //     for (int y = 0; y < _size; y++)
        //     {
        //         for (int x = 0; x < _size; x++)
        //         {
        //             var _tile = Instantiate(tilePrefab, transform);
        //             _tile.transform.localScale = _resolusionSize;

        //             Vector2 _nextPos = new Vector2(x, y) * _resolusionSize;
        //             if (!_isOddNumber) _nextPos += Vector2.one * (_resolusionSize * 0.5f);

        //             _tile.SetTilePos(x, y);
        //             _tile.name = string.Format($"({x} / {y})");
        //             _tile.transform.position = _startPos + _nextPos;
        //         }
        //     }
        // }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.DrawWireCube(transform.position, Vector2.one);
        // }
        // private void CameraResizing(float _count)
        // {
        //     Vector2 spriteSize = tilePrefab.GetTileSize() * (_count + cameraSpecing);

        //     // SpriteRenderer의 가로 크기를 기준으로 orthographic size 계산
        //     float orthoSize = spriteSize.x * 0.5f / GameCamera.aspect;

        //     // 카메라의 orthographic size 설정
        //     GameCamera.orthographicSize = orthoSize;
        // }
    }

}