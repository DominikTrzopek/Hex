using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int attackPoints;
    [SerializeField]
    private int moveRange;
    [SerializeField]
    private int attackRange;

    public int getHP()
    {
        return healthPoints;
    }
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
}
