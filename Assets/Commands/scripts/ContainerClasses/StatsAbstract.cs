using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatsAbstract : MonoBehaviour
{
    [SerializeField]
    protected int healthPoints;
    [SerializeField]
    protected int viewRange;
    protected int maxHealthPoints;
    protected int level;

    public void Start()
    {
        maxHealthPoints = healthPoints;
        level = 1;
    }

    public int getHP()
    {
        return healthPoints;
    }

    public int getMaxHP()
    {
        return maxHealthPoints;
    }

    public int getVR()
    {
        return viewRange;
    }

    public void upgradeHP()
    {
        healthPoints++;
        maxHealthPoints++;
    }

    public void upgradeVR()
    {
        viewRange++;
    }

    public void applyAttackPoints(int value)
    {
        healthPoints -= value;
    }

    public int getLevel()
    {
        return level;
    }

    public void upgrade()
    {
        level++;
    }


}
