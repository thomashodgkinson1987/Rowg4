using System;
using Godot;

public class WallTile : Tile
{

    #region Fields

    [Export] private float m_minColor = 0.25f;
    [Export] private float m_maxColor = 0.75f;

    private readonly Random m_rng;

    #endregion // Fields



    #region Constructors

    public WallTile ()
    {
        m_rng = new Random();
    }

    #endregion // Constructors



    #region Godot methods

    public override void _Ready ()
    {
        base._Ready();

        float randomColor = ((float)m_rng.NextDouble() * (m_maxColor - m_minColor)) + m_minColor;
        SetMainSpriteColor(randomColor, randomColor, randomColor, 1);
    }

    #endregion // Godot methods

}
