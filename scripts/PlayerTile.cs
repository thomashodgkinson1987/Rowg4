using System;
using System.Collections.Generic;
using Godot;

public class PlayerTile : Tile
{

    #region Nodes

    private Sprite node_bodySprite;
    private Sprite node_topSprite;
    private Sprite node_bottomsSprite;
    private Sprite node_shoesSprite;

    #endregion // Nodes



    #region Properties

    public int Health => m_health;
    public int MaxHealth => m_maxHealth;

    public int SightLength => m_sightLength;

    #endregion // Properties



    #region Fields

    [Export] private int m_health = 10;
    [Export] private int m_maxHealth = 10;

    [Export] private int m_sightLength = 4;

    private readonly Random m_rng;

    #endregion // Fields



    #region Constructors

    public PlayerTile ()
    {
        m_rng = new Random();
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

    public void SetHealth (int health, int maxHealth)
    {
        maxHealth = Mathf.Max(1, maxHealth);
        m_maxHealth = maxHealth;
        if (Health > MaxHealth)
            m_health = MaxHealth;

        health = Mathf.Clamp(health, 0, MaxHealth);
        m_health = health;
    }
    public void SetHealth (int health)
    {
        health = Mathf.Clamp(health, 0, MaxHealth);
        m_health = health;
    }
    public void SetMaxHealth (int maxHealth)
    {
        maxHealth = Mathf.Max(1, maxHealth);
        m_maxHealth = maxHealth;
        if (Health > MaxHealth)
            m_health = MaxHealth;
    }

    public void IncreaseHealth (int amount)
    {
        SetHealth(Health + amount);
    }
    public void DecreaseHealth (int amount)
    {
        SetHealth(Health - amount);
    }

    public void SetSightLength (int length)
    {
        m_sightLength = length;
    }

    #endregion // Public methods

}
