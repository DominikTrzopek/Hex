using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureStats : StatsAbstract
{
    [SerializeField]
    private int takenRange;
    [SerializeField]
    private int level;
    
    private List<GameObject> connected = new List<GameObject>();

    public int getLevel()
    {
        return level;
    }

    public int getTakenRange()
    {
        return takenRange;
    }

    public void addConnected(GameObject obj)
    {
        connected.Add(obj);
    }

    public void upgrade()
    {
        level++;
    }

}
