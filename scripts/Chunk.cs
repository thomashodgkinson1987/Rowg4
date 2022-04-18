using System.Collections.Generic;
using Godot;

public class Chunk : Node2D
{

    #region Nodes

    private Node2D node_tiles;
    private Position2D node_position2D;

    #endregion // Nodes



    #region Properties

    [Export] public int Width { get; private set; } = 16;
    [Export] public int Height { get; private set; } = 16;

    public Tile[,] TileMap => m_tileMap;
    public List<Tile> Tiles => m_tiles;

    public Vector2 Center => node_position2D.Position;
    public Vector2 GlobalCenter => node_position2D.GlobalPosition;

    #endregion // Properties



    #region Fields

    [Export] private PackedScene m_tilePackedScene;

    private readonly Tile[,] m_tileMap;
    private List<Tile> m_tiles;

    #endregion // Fields



    #region Constructors

    public Chunk ()
    {
        m_tileMap = new Tile[Height, Width];

        for (int i = 0; i < Height; i++)
            for (int j = 0; j < Width; j++)
                m_tileMap[i, j] = null;
    }

    #endregion // Constructors



    #region Godot methods

    public override void _EnterTree ()
    {
        node_tiles = GetNode<Node2D>("Tiles");
        node_position2D = GetNode<Position2D>("Center");
    }

    #endregion // Godot methods



    #region Public methods

    #endregion // Public methods

}
