using System;
using System.Collections.Generic;
using Godot;

public class EnemyTile : Tile
{

    #region Properties

    public int Health => m_health;
    public int MaxHealth => m_maxHealth;

    #endregion // Properties



    #region Fields

    [Export] private List<Texture> m_textures;

    [Export] private int m_health = 10;
    [Export] private int m_maxHealth = 10;

    private readonly Random m_rng;

    #endregion // Fields



    #region Constructors

    public EnemyTile ()
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

    #endregion // Public methods

}
