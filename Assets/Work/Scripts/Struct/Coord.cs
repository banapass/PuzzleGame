[System.Serializable]
public struct Coord
{
    public int x;
    public int y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Coord Up { get { return new Coord(0, 1); } }
    public static Coord Down { get { return new Coord(0, -1); } }
    public static Coord Left { get { return new Coord(1, 0); } }
    public static Coord Right { get { return new Coord(-1, 0); } }

    public static Coord[] FourDirection
    {
        get
        {
            Coord[] _result = new Coord[4];
            int _index = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    if (x == 0 || y == 0)
                    {
                        _result[_index] = new Coord(x, y);
                        _index++;
                    }
                }
            }
            return _result;
        }
    }
}