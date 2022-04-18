using Godot;
using System;
using System.Collections.Generic;

public class MainScene : Node2D
{

    #region Nodes

    private ViewportContainer node_viewportContainer;
    private Viewport node_viewport;

    private RichTextLabel node_messageLog;
    private RichTextLabel node_startMessage;

    private Label node_healthValueLabel;
    private Label node_maxHealthValueLabel;

    #endregion // Nodes



    #region Fields

    [Export] private PackedScene m_levelPackedScene;
    private Level m_currentLevel;

    private readonly List<string> m_messages;

    #endregion // Fields



    #region Constructors

    public MainScene ()
    {
        m_messages = new List<string>();
    }

    #endregion // Constructors



    #region Godot methods

    public override void _EnterTree ()
    {
        node_viewportContainer = GetNode<ViewportContainer>("ViewportContainer");
        node_viewport = node_viewportContainer.GetNode<Viewport>("Viewport");

        node_messageLog = GetNode<RichTextLabel>("Control/Panel/RichTextLabel");
        node_startMessage = GetNode<RichTextLabel>("Control/Panel/RichTextLabel2");

        node_healthValueLabel = GetNode<Label>("Control2/Panel/HealthValueLabel");
        node_maxHealthValueLabel = GetNode<Label>("Control2/Panel/MaxHealthValueLabel");
    }

    public override void _Ready ()
    {
        m_currentLevel = m_levelPackedScene.Instance<Level>();
        node_viewport.AddChild(m_currentLevel);

        m_currentLevel.Connect(nameof(Level.PlayerHitEnemySignal), this, nameof(OnPlayerHitEnemy));
        m_currentLevel.Connect(nameof(Level.PlayerKilledEnemySignal), this, nameof(OnPlayerKilledEnemy));
        m_currentLevel.Connect(nameof(Level.EnemyHitPlayerSignal), this, nameof(OnEnemyHitPlayer));
        m_currentLevel.Connect(nameof(Level.EnemyKilledPlayerSignal), this, nameof(OnEnemyKilledPlayer));
        m_currentLevel.Connect(nameof(Level.PlayerWalkedIntoWallSignal), this, nameof(OnPlayerWalkedIntoWall));
        m_currentLevel.Connect(nameof(Level.EnemyCameIntoViewSignal), this, nameof(OnEnemyCameIntoView));
        m_currentLevel.Connect(nameof(Level.EnemyWentOutOfViewSignal), this, nameof(OnEnemyWentOutOfView));
    }

    public override void _Input (InputEvent @event)
    {
        if (@event is InputEventKey inputEventKey && inputEventKey.Pressed /* && !inputEventKey.IsEcho() */)
        {
            uint scancode = inputEventKey.Scancode;

            if (node_startMessage.Visible)
                node_startMessage.Visible = false;
        }
    }

    #endregion // Godot methods



    #region Private methods

    private void AddMessage (string message)
    {
        m_messages.Add(message);
        node_messageLog.AppendBbcode(message);
    }

    #endregion // Private methods



    #region Callback methods

    private void OnPlayerHitEnemy (PlayerTile playerTile, EnemyTile enemyTile, int damage)
    {
        string message = $"\n[color=blue]{playerTile.TileName}[/color] hit [color=red]{enemyTile.TileName}[/color] for [color=yellow]{damage}[/color] damage";
        AddMessage(message);
    }
    private void OnPlayerKilledEnemy (PlayerTile playerTile, EnemyTile enemyTile)
    {
        string message = $"\n[color=blue]{playerTile.TileName}[/color] killed [color=red]{enemyTile.TileName}[/color]";
        AddMessage(message);
    }
    private void OnEnemyHitPlayer (EnemyTile enemyTile, PlayerTile playerTile, int damage)
    {
        string message = $"\n[color=red]{enemyTile.TileName}[/color] hit [color=blue]{playerTile.TileName}[/color] for [color=yellow]{damage}[/color] damage";
        AddMessage(message);

        node_healthValueLabel.Text = playerTile.Health.ToString();
    }
    private void OnEnemyKilledPlayer (EnemyTile enemyTile, PlayerTile playerTile)
    {
        string message = $"\n[color=red]{enemyTile.TileName}[/color] killed [color=blue]{playerTile.TileName}[/color]";
        message += "\nPress the R key to restart";
        AddMessage(message);
    }
    private void OnPlayerWalkedIntoWall (PlayerTile playerTile, WallTile wallTile)
    {
        string message = $"\n[color=blue]{playerTile.TileName}[/color] walked in to [color=gray]{wallTile.TileName}[/color]";
        AddMessage(message);
    }

    private void OnEnemyCameIntoView (EnemyTile enemyTile)
    {
        string message = $"\n[color=red]{enemyTile.TileName}[/color] came into view";
        AddMessage(message);
    }

    private void OnEnemyWentOutOfView (EnemyTile enemyTile)
    {
        string message = $"\n[color=red]{enemyTile.TileName}[/color] went out of view";
        AddMessage(message);
    }

    #endregion // Callback methods

}
