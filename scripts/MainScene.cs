using Godot;
using System;
using System.Collections.Generic;

public class MainScene : Node2D
{

    #region Nodes

    private ViewportContainer node_viewportContainer;
    private Viewport node_viewport;

    private RichTextLabel node_messageLogLabel;
    private RichTextLabel node_introMessageLabel;

    private Label node_healthValueLabel;
    private Label node_maxHealthValueLabel;

    private Label node_strengthValueLabel;
    private Label node_strengthModifierValueLabel;
    private Label node_dexterityValueLabel;
    private Label node_dexterityModifierValueLabel;
    private Label node_constitutionValueLabel;
    private Label node_constitutionModifierValueLabel;
    private Label node_intelligenceValueLabel;
    private Label node_intelligenceModifierValueLabel;
    private Label node_wisdomValueLabel;
    private Label node_wisdomModifierValueLabel;
    private Label node_charismaValueLabel;
    private Label node_charismaModifierValueLabel;

    private Label node_armorClassValueLabel;

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

        Control bottomControl = GetNode<Control>("BottomControl");

        node_messageLogLabel = bottomControl.GetNode<RichTextLabel>("Panel/MessageLogLabel");
        node_introMessageLabel = bottomControl.GetNode<RichTextLabel>("Panel/IntroMessageLabel");

        Control rightControl = GetNode<Control>("RightControl");

        node_healthValueLabel = rightControl.GetNode<Label>("Panel/HealthValueLabel");
        node_maxHealthValueLabel = rightControl.GetNode<Label>("Panel/MaxHealthValueLabel");

        node_strengthValueLabel = rightControl.GetNode<Label>("Panel/StrengthValueLabel");
        node_strengthModifierValueLabel = rightControl.GetNode<Label>("Panel/StrengthModifierValueLabel");
        node_dexterityValueLabel = rightControl.GetNode<Label>("Panel/DexterityValueLabel");
        node_dexterityModifierValueLabel = rightControl.GetNode<Label>("Panel/DexterityModifierValueLabel");
        node_constitutionValueLabel = rightControl.GetNode<Label>("Panel/ConstitutionValueLabel");
        node_constitutionModifierValueLabel = rightControl.GetNode<Label>("Panel/ConstitutionModifierValueLabel");
        node_intelligenceValueLabel = rightControl.GetNode<Label>("Panel/IntelligenceValueLabel");
        node_intelligenceModifierValueLabel = rightControl.GetNode<Label>("Panel/IntelligenceModifierValueLabel");
        node_wisdomValueLabel = rightControl.GetNode<Label>("Panel/WisdomValueLabel");
        node_wisdomModifierValueLabel = rightControl.GetNode<Label>("Panel/WisdomModifierValueLabel");
        node_charismaValueLabel = rightControl.GetNode<Label>("Panel/CharismaValueLabel");
        node_charismaModifierValueLabel = rightControl.GetNode<Label>("Panel/CharismaModifierValueLabel");

        node_armorClassValueLabel = rightControl.GetNode<Label>("Panel/ACValueLabel");
    }

    public override void _Ready ()
    {
        m_currentLevel = m_levelPackedScene.Instance<Level>();
        node_viewport.AddChild(m_currentLevel);

        m_currentLevel.Connect(nameof(Level.PlayerStatsChangedSignal), this, nameof(OnPlayerStatsChanged));
        m_currentLevel.Connect(nameof(Level.PlayerWalkedIntoWallSignal), this, nameof(OnPlayerWalkedIntoWall));

        m_currentLevel.Connect(nameof(Level.PlayerRolledAttackAgainstEnemySignal), this, nameof(OnPlayerRolledAttackAgainstEnemy));
        m_currentLevel.Connect(nameof(Level.PlayerHitEnemySignal), this, nameof(OnPlayerHitEnemy));
        m_currentLevel.Connect(nameof(Level.PlayerMissedEnemySignal), this, nameof(OnPlayerMissedEnemy));
        m_currentLevel.Connect(nameof(Level.PlayerRolledDamageAgainstEnemySignal), this, nameof(OnPlayerRolledDamageAgainstEnemy));
        m_currentLevel.Connect(nameof(Level.PlayerDamagedEnemySignal), this, nameof(OnPlayerDamagedEnemy));
        m_currentLevel.Connect(nameof(Level.PlayerKilledEnemySignal), this, nameof(OnPlayerKilledEnemy));

        m_currentLevel.Connect(nameof(Level.EnemyRolledAttackAgainstPlayerSignal), this, nameof(OnEnemyRolledAttackAgainstPlayer));
        m_currentLevel.Connect(nameof(Level.EnemyHitPlayerSignal), this, nameof(OnEnemyHitPlayer));
        m_currentLevel.Connect(nameof(Level.EnemyMissedPlayerSignal), this, nameof(OnEnemyMissedPlayer));
        m_currentLevel.Connect(nameof(Level.EnemyRolledDamageAgainstPlayerSignal), this, nameof(OnEnemyRolledDamageAgainstPlayer));
        m_currentLevel.Connect(nameof(Level.EnemyDamagedPlayerSignal), this, nameof(OnEnemyDamagedPlayer));
        m_currentLevel.Connect(nameof(Level.EnemyKilledPlayerSignal), this, nameof(OnEnemyKilledPlayer));

        m_currentLevel.Connect(nameof(Level.EnemyCameIntoViewSignal), this, nameof(OnEnemyCameIntoView));
        m_currentLevel.Connect(nameof(Level.EnemyWentOutOfViewSignal), this, nameof(OnEnemyWentOutOfView));
    }

    public override void _Input (InputEvent @event)
    {
        if (@event is InputEventKey inputEventKey && inputEventKey.Pressed /* && !inputEventKey.IsEcho() */)
        {
            uint scancode = inputEventKey.Scancode;

            if (node_introMessageLabel.Visible)
            {
                node_introMessageLabel.Visible = false;
            }
        }
    }

    #endregion // Godot methods



    #region Private methods

    private void AddMessage (string message)
    {
        m_messages.Add(message);
        node_messageLogLabel.AppendBbcode(message);
    }

    #endregion // Private methods



    #region Callback methods

    private void OnPlayerStatsChanged (PlayerTile playerTile)
    {
        node_healthValueLabel.Text = playerTile.CurrentHealth.ToString();
        node_maxHealthValueLabel.Text = playerTile.MaxHealth.ToString();

        node_strengthValueLabel.Text = playerTile.BaseStrength.ToString();
        node_strengthModifierValueLabel.Text = playerTile.BaseStrengthModifier.ToString();
        node_dexterityValueLabel.Text = playerTile.BaseDexterity.ToString();
        node_dexterityModifierValueLabel.Text = playerTile.BaseDexterityModifier.ToString();
        node_constitutionValueLabel.Text = playerTile.BaseConstitution.ToString();
        node_constitutionModifierValueLabel.Text = playerTile.BaseConstitutionModifier.ToString();
        node_intelligenceValueLabel.Text = playerTile.BaseIntelligence.ToString();
        node_intelligenceModifierValueLabel.Text = playerTile.BaseIntelligenceModifier.ToString();
        node_wisdomValueLabel.Text = playerTile.BaseWisdom.ToString();
        node_wisdomModifierValueLabel.Text = playerTile.BaseWisdomModifier.ToString();
        node_charismaValueLabel.Text = playerTile.BaseCharisma.ToString();
        node_charismaModifierValueLabel.Text = playerTile.BaseCharismaModifier.ToString();

        node_armorClassValueLabel.Text = playerTile.ArmorClass.ToString();
    }

    private void OnPlayerWalkedIntoWall (PlayerTile playerTile, WallTile wallTile)
    {
        string message = $"\n[color=blue]{playerTile.TileName}[/color] walked in to [color=gray]{wallTile.TileName}[/color]";
        AddMessage(message);
    }

    private void OnPlayerRolledAttackAgainstEnemy (PlayerTile playerTile, EnemyTile enemyTile, List<int> attackRollResults, int attackRollTotal)
    {
        string message = $"\n[color=blue]{playerTile.TileName}[/color] rolled attack against [color=red]{enemyTile.TileName}[/color] [color=purple](";
        for (int i = 0; i < attackRollResults.Count; i++)
        {
            message += $"{attackRollResults[i]}";
            if (i < attackRollResults.Count - 1)
                message += " + ";
        }
        message += $" = {attackRollTotal})[/color]";
        AddMessage(message);
    }
    private void OnPlayerHitEnemy (PlayerTile playerTile, EnemyTile enemyTile, int attackRollTotal)
    {
        string message = $"\n[color=blue]{playerTile.TileName}[/color] hit [color=red]{enemyTile.TileName}[/color] ([color=yellow]{attackRollTotal} vs {enemyTile.ArmorClass}[/color])";
        AddMessage(message);
    }
    private void OnPlayerMissedEnemy (PlayerTile playerTile, EnemyTile enemyTile, int attackRollTotal)
    {
        string message = $"\n[color=blue]{playerTile.TileName}[/color] missed [color=red]{enemyTile.TileName}[/color] ([color=yellow]{attackRollTotal} vs {enemyTile.ArmorClass}[/color])";
        AddMessage(message);
    }
    private void OnPlayerRolledDamageAgainstEnemy (PlayerTile playerTile, EnemyTile enemyTile, List<int> damageRollResults, int damageRollTotal)
    {
        string message = $"\n[color=blue]{playerTile.TileName}[/color] rolled damage against [color=red]{enemyTile.TileName}[/color] [color=purple](";
        for (int i = 0; i < damageRollResults.Count; i++)
        {
            message += $"{damageRollResults[i]}";
            if (i < damageRollResults.Count - 1)
                message += " + ";
        }
        message += $" = {damageRollTotal})[/color]";
        AddMessage(message);
    }
    private void OnPlayerDamagedEnemy (PlayerTile playerTile, EnemyTile enemyTile, int damageRollTotal)
    {
        string message = $"\n[color=blue]{playerTile.TileName}[/color] damaged [color=red]{enemyTile.TileName}[/color] [color=yellow]({damageRollTotal})[/color]";
        AddMessage(message);
    }
    private void OnPlayerKilledEnemy (PlayerTile playerTile, EnemyTile enemyTile)
    {
        string message = $"\n[color=blue]{playerTile.TileName}[/color] killed [color=red]{enemyTile.TileName}[/color]";
        AddMessage(message);
    }

    private void OnEnemyRolledAttackAgainstPlayer (PlayerTile playerTile, EnemyTile enemyTile, List<int> attackRollResults, int attackRollTotal)
    {
        string message = $"\n[color=red]{enemyTile.TileName}[/color] rolled attack against [color=blue]{playerTile.TileName}[/color] [color=purple](";
        for (int i = 0; i < attackRollResults.Count; i++)
        {
            message += $"{attackRollResults[i]}";
            if (i < attackRollResults.Count - 1)
                message += " + ";
        }
        message += $" = {attackRollTotal})[/color]";
        AddMessage(message);
    }
    private void OnEnemyHitPlayer (PlayerTile playerTile, EnemyTile enemyTile, int attackRollTotal)
    {
        string message = $"\n[color=red]{enemyTile.TileName}[/color] hit [color=blue]{playerTile.TileName}[/color] ([color=yellow]{attackRollTotal} vs {playerTile.ArmorClass}[/color])";
        AddMessage(message);
    }
    private void OnEnemyMissedPlayer (PlayerTile playerTile, EnemyTile enemyTile, int attackRollTotal)
    {
        string message = $"\n[color=red]{enemyTile.TileName}[/color] missed [color=blue]{playerTile.TileName}[/color] ([color=yellow]{attackRollTotal} vs {playerTile.ArmorClass}[/color])";
        AddMessage(message);
    }
    private void OnEnemyRolledDamageAgainstPlayer (PlayerTile playerTile, EnemyTile enemyTile, List<int> damageRollResults, int damageRollTotal)
    {
        string message = $"\n[color=red]{enemyTile.TileName}[/color] rolled damage against [color=blue]{playerTile.TileName}[/color] [color=purple](";
        for (int i = 0; i < damageRollResults.Count; i++)
        {
            message += $"{damageRollResults[i]}";
            if (i < damageRollResults.Count - 1)
                message += " + ";
        }
        message += $" = {damageRollTotal})[/color]";
        AddMessage(message);
    }
    private void OnEnemyDamagedPlayer (PlayerTile playerTile, EnemyTile enemyTile, int damageRollTotal)
    {
        string message = $"\n[color=red]{enemyTile.TileName}[/color] damaged [color=blue]{playerTile.TileName}[/color] [color=yellow]({damageRollTotal})[/color]";
        AddMessage(message);

        node_healthValueLabel.Text = playerTile.CurrentHealth.ToString();
    }
    private void OnEnemyKilledPlayer (PlayerTile playerTile, EnemyTile enemyTile)
    {
        string message = $"\n[color=red]{enemyTile.TileName}[/color] killed [color=blue]{playerTile.TileName}[/color]";
        message += "\nPress the R key to restart";
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
