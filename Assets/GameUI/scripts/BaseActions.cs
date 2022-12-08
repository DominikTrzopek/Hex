using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseActions : ActionsAbstract, IPlayerObjectHandler
{
    public static BaseActions instance { get; private set; }
    GameObject obj;
    public GameObject uiImage;
    public GameObject structure;

    public void SetObj(GameObject toSet)
    {
        obj = toSet;
    }

    public Vector3 GetObjPosition()
    {
        return obj.transform.position;
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

    public void InstantiateUnit()
    {
        SelectPlayerObj.command = CommandEnum.INSTANTIANE_UNIT;
        PerformAction(new InitUnitHandler(obj, uiImage));
    }

    public void MakeBank()
    {
        SelectPlayerObj.command = CommandEnum.MAKE_BANK;
        PerformAction(new MakeBankHandler(obj));
    }

    public void InstantiateStructure()
    {
        SelectPlayerObj.command = CommandEnum.INSTANTIANE_STRUCTURE;
        PerformAction(new InitStructureHandler(obj));
    }

}
