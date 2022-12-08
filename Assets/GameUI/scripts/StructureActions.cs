using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureActions : ActionsAbstract, IPlayerObjectHandler
{
    public static StructureActions instance { get; private set; }
    GameObject obj;

    public void SetObj(GameObject toSet)
    {
        obj = toSet;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void InstantiateStructure()
    {
        SelectPlayerObj.command = CommandEnum.INSTANTIANE_STRUCTURE;
        PerformAction(new InitStructureHandler(obj));
    }

}
