using UnityEngine;

public class UnitStats : StatsAbstract
{
    [SerializeField]
    private int attackPoints;
    [SerializeField]
    private int moveRange;
    [SerializeField]
    private int attackRange;

    public int GetAP()
    {
        return attackPoints;
    }
    public int GetMR()
    {
        return moveRange;
    }
    public int GetAR()
    {
        return attackRange;
    }
 
    public void UpgradeAP()
    {
        attackPoints++;
    }
    public void UpgradeMR()
    {
        moveRange++;
    }
    public void UpgradeAR()
    {
        attackRange++;
    }

}
