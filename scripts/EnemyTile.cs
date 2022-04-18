using System.Collections.Generic;
using Godot;

public class EnemyTile : ActorTile
{

    #region Properties

    #endregion // Properties



    #region Fields

    [Export] private List<Texture> m_textures;

    #endregion // Fields



    #region Constructors

    public EnemyTile () : base()
    {

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



    #region Public methods

    #endregion // Public methods

}
