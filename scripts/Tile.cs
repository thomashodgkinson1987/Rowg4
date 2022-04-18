using Godot;

public class Tile : Node2D
{

    #region Nodes

    protected Sprite node_backgroundSprite;
    protected Sprite node_mainSprite;
    protected Sprite node_foregroundSprite;
    protected Position2D node_position2D;

    #endregion // Nodes



    #region Properties

    [Export] public string TileName { get; private set; } = "TileName";

    public Vector2 Center => node_position2D.Position;
    public Vector2 GlobalCenter => node_position2D.GlobalPosition;

    #endregion // Properties



    #region Godot methods

    public override void _EnterTree ()
    {
        node_backgroundSprite = GetNode<Sprite>("BackgroundSprite");
        node_mainSprite = GetNode<Sprite>("MainSprite");
        node_foregroundSprite = GetNode<Sprite>("ForegroundSprite");
        node_position2D = GetNode<Position2D>("Center");
    }

    #endregion // Godot methods



    #region Public methods

    public void SetTileName (string tileName)
    {
        TileName = tileName;
    }

    public void SetBackgroundSpriteVisible (bool visible)
    {
        node_backgroundSprite.Visible = visible;
    }
    public void ToggleBackgroundSpriteVisible ()
    {
        SetBackgroundSpriteVisible(!node_backgroundSprite.Visible);
    }

    public void SetBackgroundSpriteColor (float r, float g, float b, float a)
    {
        node_backgroundSprite.SelfModulate = new Color(r, g, b, a);
    }
    public void SetBackgroundSpriteColor (Color color)
    {
        SetBackgroundSpriteColor(color.r, color.g, color.b, color.a);
    }
    public void SetBackgroundSpriteColorR (float r)
    {
        SetBackgroundSpriteColor(r, node_backgroundSprite.SelfModulate.g, node_backgroundSprite.SelfModulate.b, node_backgroundSprite.SelfModulate.a);
    }
    public void SetBackgroundSpriteColorG (float g)
    {
        SetBackgroundSpriteColor(node_backgroundSprite.SelfModulate.r, g, node_backgroundSprite.SelfModulate.b, node_backgroundSprite.SelfModulate.a);
    }
    public void SetBackgroundSpriteColorB (float b)
    {
        SetBackgroundSpriteColor(node_backgroundSprite.SelfModulate.r, node_backgroundSprite.SelfModulate.g, b, node_backgroundSprite.SelfModulate.a);
    }
    public void SetBackgroundSpriteColorA (float a)
    {
        SetBackgroundSpriteColor(node_backgroundSprite.SelfModulate.r, node_backgroundSprite.SelfModulate.g, node_backgroundSprite.SelfModulate.b, a);
    }

    public void SetMainSpriteVisible (bool visible)
    {
        node_mainSprite.Visible = visible;
    }
    public void ToggleMainSpriteVisible ()
    {
        SetMainSpriteVisible(!node_mainSprite.Visible);
    }

    public void SetMainSpriteColor (float r, float g, float b, float a)
    {
        node_mainSprite.SelfModulate = new Color(r, g, b, a);
    }
    public void SetMainSpriteColor (Color color)
    {
        SetMainSpriteColor(color.r, color.g, color.b, color.a);
    }
    public void SetMainSpriteColorR (float r)
    {
        SetMainSpriteColor(r, node_mainSprite.SelfModulate.g, node_mainSprite.SelfModulate.b, node_mainSprite.SelfModulate.a);
    }
    public void SetMainSpriteColorG (float g)
    {
        SetMainSpriteColor(node_mainSprite.SelfModulate.r, g, node_mainSprite.SelfModulate.b, node_mainSprite.SelfModulate.a);
    }
    public void SetMainSpriteColorB (float b)
    {
        SetMainSpriteColor(node_mainSprite.SelfModulate.r, node_mainSprite.SelfModulate.g, b, node_mainSprite.SelfModulate.a);
    }
    public void SetMainSpriteColorA (float a)
    {
        SetMainSpriteColor(node_mainSprite.SelfModulate.r, node_mainSprite.SelfModulate.g, node_mainSprite.SelfModulate.b, a);
    }

    public void SetForegroundSpriteVisible (bool visible)
    {
        node_foregroundSprite.Visible = visible;
    }
    public void ToggleForegroundSpriteVisible ()
    {
        SetForegroundSpriteVisible(!node_foregroundSprite.Visible);
    }

    public void SetForegroundSpriteColor (float r, float g, float b, float a)
    {
        node_foregroundSprite.SelfModulate = new Color(r, g, b, a);
    }
    public void SetForegroundSpriteColor (Color color)
    {
        SetForegroundSpriteColor(color.r, color.g, color.b, color.a);
    }
    public void SetForegroundSpriteColorR (float r)
    {
        SetForegroundSpriteColor(r, node_foregroundSprite.SelfModulate.g, node_foregroundSprite.SelfModulate.b, node_foregroundSprite.SelfModulate.a);
    }
    public void SetForegroundSpriteColorG (float g)
    {
        SetForegroundSpriteColor(node_foregroundSprite.SelfModulate.r, g, node_foregroundSprite.SelfModulate.b, node_foregroundSprite.SelfModulate.a);
    }
    public void SetForegroundSpriteColorB (float b)
    {
        SetForegroundSpriteColor(node_foregroundSprite.SelfModulate.r, node_foregroundSprite.SelfModulate.g, b, node_foregroundSprite.SelfModulate.a);
    }
    public void SetForegroundSpriteColorA (float a)
    {
        SetForegroundSpriteColor(node_foregroundSprite.SelfModulate.r, node_foregroundSprite.SelfModulate.g, node_foregroundSprite.SelfModulate.b, a);
    }

    #endregion // Public methods

}
