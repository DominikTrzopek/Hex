using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatsAbstract : MonoBehaviour
{
    [SerializeField]
    private int healthPoints;

    [SerializeField]
    private int viewRange;

    public int getHP()
    {
        return healthPoints;
    }

    public int getVR()
    {
        return viewRange;
    }

    public void upgradeHP()
    {
        healthPoints++;
    }

    public void upgradeVR()
    {
        viewRange++;
    }
}
