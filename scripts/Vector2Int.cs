public struct Vector2Int
{

    #region Static

    public static Vector2Int Zero = new Vector2Int(0, 0);
    public static Vector2Int One = new Vector2Int(1, 1);
    public static Vector2Int Left = new Vector2Int(-1, 0);
    public static Vector2Int Right = new Vector2Int(1, 0);
    public static Vector2Int Up = new Vector2Int(0, -1);
    public static Vector2Int Down = new Vector2Int(0, 1);

    #endregion // Static



    #region Operator overloads

    public static Vector2Int operator + (Vector2Int a)
        => new Vector2Int(a.X, a.Y);

    public static Vector2Int operator - (Vector2Int a)
        => new Vector2Int(-a.X, -a.Y);

    public static Vector2Int operator + (Vector2Int a, Vector2Int b)
        => new Vector2Int(a.X + b.X, a.Y + b.Y);

    public static Vector2Int operator - (Vector2Int a, Vector2Int b)
        => new Vector2Int(a.X - b.X, a.Y - b.Y);

    #endregion // Operator overloads



    #region Properties

    public int X { get; set; }
    public int Y { get; set; }

    #endregion // Properties



    #region Constructors

    public Vector2Int (int x, int y)
    {
        X = x;
        Y = y;
    }

    #endregion // Constructors

}
