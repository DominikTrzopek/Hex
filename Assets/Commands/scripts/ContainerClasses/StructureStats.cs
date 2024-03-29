using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureStats : StatsAbstract
{
    [SerializeField]
    private int takenRange;

    private List<GameObject> connected = new List<GameObject>();
    public string parentId;

    public int GetTakenRange()
    {
        return takenRange;
    }

    public void AddConnected(GameObject obj)
    {
        connected.Add(obj);
    }

    public void DestroyAllConnected()
    {
        foreach(GameObject obj in connected)
        {
            if(obj != null)
                Destroy(obj);
        }
    }

}
