using System;
using System.Collections.Generic;

public class DiceRoller
{

    public enum EDiceType { D100 = 100, D20 = 20, D12 = 12, D10 = 10, D8 = 8, D6 = 6, D4 = 4 }

    private readonly Random m_rng;

    private readonly List<int> m_countList;
    private readonly List<EDiceType?> m_diceTypeList;
    private readonly List<int?> m_numbersList;

    private int m_steps;

    public DiceRoller ()
    {
        m_rng = new Random();

        m_countList = new List<int>();
        m_diceTypeList = new List<EDiceType?>();
        m_numbersList = new List<int?>();

        m_steps = 0;
    }

    public DiceRoller Add (int count, EDiceType diceType)
    {
        count = count <= 0 ? 1 : count;

        m_countList.Add(count);
        m_diceTypeList.Add(diceType);
        m_numbersList.Add(null);

        m_steps++;

        return this;
    }
    public DiceRoller Add (int count, int number)
    {
        count = count <= 0 ? 1 : count;

        m_countList.Add(count);
        m_diceTypeList.Add(null);
        m_numbersList.Add(number);

        m_steps++;

        return this;
    }

    public (List<int> results, int total) Roll ()
    {
        List<int> results = new List<int>();
        int total = 0;

        for (int i = 0; i < m_steps; i++)
        {
            EDiceType? diceTypeEntry = m_diceTypeList[i];
            int? numberEntry = m_numbersList[i];
            int times = m_countList[i];

            for (int j = 0; j < times; j++)
            {
                if (diceTypeEntry != null && diceTypeEntry is EDiceType diceType)
                {
                    int value;

                    switch (diceType)
                    {
                        case EDiceType.D100:
                            value = m_rng.Next(1, 101);
                            results.Add(value);
                            total += value;
                            break;
                        case EDiceType.D20:
                            value = m_rng.Next(1, 21);
                            results.Add(value);
                            total += value;
                            break;
                        case EDiceType.D10:
                            value = m_rng.Next(1, 11);
                            results.Add(value);
                            total += value;
                            break;
                        case EDiceType.D8:
                            value = m_rng.Next(1, 9);
                            results.Add(value);
                            total += value;
                            break;
                        case EDiceType.D6:
                            value = m_rng.Next(1, 7);
                            results.Add(value);
                            total += value;
                            break;
                        case EDiceType.D4:
                            value = m_rng.Next(1, 5);
                            results.Add(value);
                            total += value;
                            break;
                        default:
                            break;
                    }
                }
                else if (numberEntry != null && numberEntry is int number)
                {
                    results.Add(number);
                    total += number;
                }
            }
        }

        m_diceTypeList.Clear();
        m_numbersList.Clear();
        m_countList.Clear();
        m_steps = 0;

        return (results, total);
    }

}
