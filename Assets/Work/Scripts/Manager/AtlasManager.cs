using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


public class AtlasManager : Singleton<AtlasManager>
{
    [SerializeField] SpriteAtlas atlas;

    public Sprite GetSprite(string _name)
    {
        if (atlas == null) return null;

        return atlas.GetSprite(_name);
    }
}
