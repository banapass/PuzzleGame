using UnityEngine;

[System.Serializable]
public struct BlockInfo
{
    public string BlockIMG;
    public Coord[] BlockCoord; //{ get; set; }


    public Vector2 GetSize()
    {

        return new Vector2(GetCountX() + 1, GetCountY() + 1);

    }
    private int GetCountX()
    {
        int _result = 0;

        for (int i = 0; i < BlockCoord.Length; ++i)
        {
            if (BlockCoord[i].x == 0) continue;

            _result++;
        }

        return _result;
    }

    private int GetCountY()
    {
        int _result = 0;

        for (int i = 0; i < BlockCoord.Length; ++i)
        {
            if (BlockCoord[i].y == 0) continue;
            _result++;
        }
        return _result;
    }
}