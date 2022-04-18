using System;
using System.Collections.Generic;
using Godot;

public class FloorTile : Tile
{

    #region Fields

    [Export] private List<Texture> m_textures;

    private readonly Random m_rng;

    #endregion // Fields



    #region Constructors

    public FloorTile ()
    {
        m_rng = new Random();
    }

    #endregion // Constructors



    #region Godot methods

    public override void _Ready ()
    {
        base._Ready();

        if (m_textures != null && m_textures.Count > 0)
        {
            int index = m_rng.Next(m_textures.Count);
            node_mainSprite.Texture = m_textures[index];
        }
    }

    #endregion // Godot methods

}
