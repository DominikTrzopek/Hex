using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : StatsAbstract
{
    [SerializeField]
    private int attackPoints;
    [SerializeField]
    private int moveRange;
    [SerializeField]
    private int attackRange;

    public int getAP()
    {
        return attackPoints;
    }
    public int getMR()
    {
        return moveRange;
    }
    public int getAR()
    {
        return attackRange;
    }
 
    public void upgradeAP()
    {
        attackPoints++;
    }
    public void upgradeMR()
    {
        moveRange++;
    }
    public void upgradeAR()
    {
        attackRange++;
    }

}
