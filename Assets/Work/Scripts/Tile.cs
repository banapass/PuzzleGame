namespace PuzzleGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Tile : MonoBehaviour
    {
        [field: SerializeField]
        public Vector2Int TilePos { get; private set; }
        public Block InBlock { get; private set; }

        private SpriteRenderer spriteRender;
        private void Awake()
        {
            spriteRender = GetComponent<SpriteRenderer>();
        }

        public void SetTilePos(int _x, int _y)
        {
            TilePos = new Vector2Int(_x, _y);
        }
        public void SetBlock(Block _block)
        {
            InBlock = _block;
        }
        public Vector2 GetTileSize()
        {
            if (spriteRender == null) spriteRender = GetComponent<SpriteRenderer>();
            return spriteRender.size;
        }
    }

}