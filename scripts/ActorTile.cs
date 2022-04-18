using System;
using Godot;
using static DiceRoller;

public class ActorTile : Tile
{

    #region Properties

    public int CurrentHealth => m_currentHealth;
    public int MaxHealth => m_maxHealth;

    public int BaseStrength => m_baseStrength;
    public int BaseDexterity => m_baseDexterity;
    public int BaseConstitution => m_baseConstitution;
    public int BaseIntelligence => m_baseIntelligence;
    public int BaseWisdom => m_baseWisdom;
    public int BaseCharisma => m_baseCharisma;

    public int BaseStrengthModifier => Mathf.FloorToInt((BaseStrength - 10) / 2f);
    public int BaseDexterityModifier => Mathf.FloorToInt((BaseDexterity - 10) / 2f);
    public int BaseConstitutionModifier => Mathf.FloorToInt((BaseConstitution - 10) / 2f);
    public int BaseIntelligenceModifier => Mathf.FloorToInt((BaseIntelligence - 10) / 2f);
    public int BaseWisdomModifier => Mathf.FloorToInt((BaseWisdom - 10) / 2f);
    public int BaseCharismaModifier => Mathf.FloorToInt((BaseCharisma - 10) / 2f);

    public int HitDiceCount => m_hitDiceCount;
    public EDiceType HitDice => m_hitDice;

    public int ArmorClass => 10 + /* armor bonus + shield bonus + */ BaseDexterityModifier /* + other modifiers */;

    public int BaseAttackBonus => m_baseAttackBonus;
    public int MeleeAttackBonus => BaseAttackBonus + BaseStrengthModifier /* + size modifier */;
    //public int RangedAttackBonus => BaseAttackBonus + BaseDexterityModifier /* + size modifier + range penalty */;

    #endregion // Properties



    #region Fields

    [Export] private int m_currentHealth = 10;
    [Export] private int m_maxHealth = 10;

    [Export] private int m_baseStrength = 10;
    [Export] private int m_baseDexterity = 10;
    [Export] private int m_baseConstitution = 10;
    [Export] private int m_baseIntelligence = 10;
    [Export] private int m_baseWisdom = 10;
    [Export] private int m_baseCharisma = 10;

    [Export] private int m_hitDiceCount = 1;
    [Export] private EDiceType m_hitDice = EDiceType.D10;

    [Export] private int m_baseAttackBonus = 1;

    protected readonly Random m_rng;

    #endregion // Fields



    #region Constructors

    public ActorTile ()
    {
        m_rng = new Random();
    }

    #endregion // Constructors



    #region Godot methods

    #endregion // Godot methods



    #region Public methods

    public void SetHealth (int health, int maxHealth)
    {
        maxHealth = Mathf.Max(1, maxHealth);
        m_maxHealth = maxHealth;
        if (CurrentHealth > MaxHealth)
            m_currentHealth = MaxHealth;

        health = Mathf.Clamp(health, 0, MaxHealth);
        m_currentHealth = health;
    }
    public void SetHealth (int health)
    {
        health = Mathf.Clamp(health, 0, MaxHealth);
        m_currentHealth = health;
    }
    public void SetMaxHealth (int maxHealth)
    {
        maxHealth = Mathf.Max(1, maxHealth);
        m_maxHealth = maxHealth;
        if (CurrentHealth > MaxHealth)
            m_currentHealth = MaxHealth;
    }

    public void IncreaseHealth (int amount)
    {
        SetHealth(CurrentHealth + amount);
    }
    public void DecreaseHealth (int amount)
    {
        SetHealth(CurrentHealth - amount);
    }

    public void SetBaseStrength (int baseStrength)
    {
        m_baseStrength = baseStrength;
    }
    public void SetBaseDexterity (int baseDexterity)
    {
        m_baseDexterity = baseDexterity;
    }
    public void SetBaseConstitution (int baseConstitution)
    {
        m_baseConstitution = baseConstitution;
    }
    public void SetBaseIntelligence (int baseIntelligence)
    {
        m_baseIntelligence = baseIntelligence;
    }
    public void SetBaseWisdom (int baseWisdom)
    {
        m_baseWisdom = baseWisdom;
    }
    public void SetBaseCharisma (int baseCharisma)
    {
        m_baseCharisma = baseCharisma;
    }

    public void SetBaseAttackBonus (int baseAttackBonus)
    {
        m_baseAttackBonus = baseAttackBonus;
    }

    public void SetHitDice (int count, EDiceType diceType)
    {
        m_hitDiceCount = count;
        m_hitDice = diceType;
    }

    #endregion // Public methods

}
