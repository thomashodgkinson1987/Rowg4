using Godot;

public class PlayerTile : ActorTile
{

    #region Nodes

    private Sprite node_bodySprite;
    private Sprite node_topSprite;
    private Sprite node_bottomsSprite;
    private Sprite node_shoesSprite;

    #endregion // Nodes



    #region Properties

    public int SightLength => m_sightLength;

    #endregion // Properties



    #region Fields

    [Export] private int m_sightLength = 4;

    #endregion // Fields



    #region Constructors

    public PlayerTile () : base()
    {

    }

    #endregion // Constructors



    #region Godot methods

    public override void _EnterTree ()
    {
        base._EnterTree();

        node_bodySprite = GetNode<Sprite>("BodySprite");
        node_topSprite = GetNode<Sprite>("TopSprite");
        node_bottomsSprite = GetNode<Sprite>("BottomsSprite");
        node_shoesSprite = GetNode<Sprite>("ShoesSprite");
    }

    public override void _Ready ()
    {
        base._Ready();

        Sprite[] sprites = new Sprite[] { node_bodySprite, node_topSprite, node_bottomsSprite, node_shoesSprite };
        for (int i = 0; i < sprites.Length; i++)
        {
            Sprite sprite = sprites[i];
            float r = (float)m_rng.NextDouble();
            float g = (float)m_rng.NextDouble();
            float b = (float)m_rng.NextDouble();
            sprite.SelfModulate = new Color(r, g, b, 1);
        }
    }

    #endregion // Godot methods



    #region Public methods

    public void SetSightLength (int length)
    {
        m_sightLength = length;
    }

    #endregion // Public methods

}
