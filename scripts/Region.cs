using Godot;
using System.Collections.Generic;

public class Region : Node2D
{

    #region Nodes

    private Node2D node_chunks;
    private Position2D node_position2D;

    #endregion // Nodes



    #region Properties

    [Export] public int Width { get; private set; } = 16;
    [Export] public int Height { get; private set; } = 16;

    public Chunk[,] ChunkMap => m_chunkMap;
    public List<Chunk> Chunks => m_chunks;

    public Vector2 Center => node_position2D.Position;
    public Vector2 GlobalCenter => node_position2D.GlobalPosition;

    #endregion // Properties



    #region Fields

    [Export] private PackedScene m_chunkPackedScene;

    private readonly Chunk[,] m_chunkMap;
    private readonly List<Chunk> m_chunks;

    #endregion // Fields



    #region Constructors

    public Region ()
    {
        m_chunkMap = new Chunk[Height, Width];

        for (int chunkY = 0; chunkY < Height; chunkY++)
            for (int chunkX = 0; chunkX < Width; chunkX++)
                m_chunkMap[chunkX, chunkY] = null;
    }

    #endregion // Constructors



    #region Godot methods

    public override void _EnterTree ()
    {
        node_chunks = GetNode<Node2D>("Chunks");
        node_position2D = GetNode<Position2D>("Center");
    }

    public override void _Ready ()
    {
        for (int chunkY = 0; chunkY < Height; chunkY++)
            for (int chunkX = 0; chunkX < Width; chunkX++)
                m_chunkMap[chunkX, chunkY] = null;
    }

    #endregion // Godot methods



    #region Public methods

    #endregion // Public methods

}
