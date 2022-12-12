using UnityEngine;

public abstract class StatsAbstract : MonoBehaviour
{
    [SerializeField]
    protected int healthPoints;
    [SerializeField]
    protected int viewRange;
    protected int maxHealthPoints;
    protected int level;
    protected int receivedAttackPoints;

    public void Start()
    {
        maxHealthPoints = healthPoints;
        level = 1;
    }

    public int GetHP()
    {
        return healthPoints;
    }

    public int GetMaxHP()
    {
        return maxHealthPoints;
    }

    public int GetVR()
    {
        return viewRange;
    }

    public void UpgradeHP()
    {
        healthPoints++;
        maxHealthPoints++;
    }

    public void UpgradeVR()
    {
        viewRange++;
    }

    public void ApplyAttackPoints(int value)
    {
        healthPoints -= value;
    }

    public void ApplyReceivedAttack()
    {
        healthPoints -= receivedAttackPoints;
    }

    public int GetLevel()
    {
        return level;
    }

    public void Upgrade()
    {
        level++;
    }

    public void AddToReceived(int value)
    {
        receivedAttackPoints += value;
    }


}
