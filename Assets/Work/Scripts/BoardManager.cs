namespace PuzzleGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BoardManager : MonoBehaviour
    {
        [field: SerializeField]
        public Camera GameCamera { get; private set; }

        [SerializeField] Tile tilePrefab;
        [SerializeField] int createCount;

        // Start is called before the first frame update
        void Start()
        {
            CreateBoard(createCount);
            CameraResizing(createCount);
        }

        private void CreateBoard(int _size)
        {
            bool _isOddNumber = _size % 2 != 0;
            float _start = _size / 2;

            Vector2 _startPos = new Vector2(-_start, -_start);
            float _count = 0;
            float _maxColorCount = Mathf.Pow(_size, 2);

            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    var _tile = Instantiate(tilePrefab, transform);
                    Vector2 _tileSize = _tile.GetTileSize();
                    Vector2 _nextPos = new Vector2(x, y) * _tileSize;

                    _tile.SetTilePos(x, y);
                    _tile.name = string.Format($"({x} / {y})");
                    if (!_isOddNumber) _nextPos += Vector2.one * 0.5f;
                    // Debug.Log($"Count : {_count} / MaxCount : {_maxColorCount} / Amount : {_count / _maxColorCount}");
                    // _tile.color = Color.Lerp(Color.white, Color.black, _count / _maxColorCount);
                    _tile.transform.position = _startPos + _nextPos;
                    _count++;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, Vector2.one);
        }
        private void CameraResizing(int _count)
        {
            Vector2 spriteSize = tilePrefab.GetTileSize() * (_count + 2);

            // SpriteRenderer의 가로 크기를 기준으로 orthographic size 계산
            float orthoSize = spriteSize.x * 0.5f / GameCamera.aspect;

            // 카메라의 orthographic size 설정
            GameCamera.orthographicSize = orthoSize;
        }
    }

}