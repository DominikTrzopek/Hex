using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActions : ActionsAbstract, IPlayerObjectHandler
{
    public static UnitActions instance { get; private set; }
    GameObject obj;
    private List<GameObject> objInRange;

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

    public void MoveUnit()
    {
        SelectPlayerObj.command = CommandEnum.MOVE;
        PerformAction(new MoveHandler(obj));
    }
}
