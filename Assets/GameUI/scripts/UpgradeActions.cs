using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeActions : ActionsAbstract, IPlayerObjectHandler
{
    public static UpgradeActions instance { get; private set; }
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

    public void UpgradeAttackPoints()
    {
        SelectPlayerObj.command = CommandEnum.UPGRADE_AP;
        PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_AP));
    }

    public void UpgradeAttackRange()
    {
        SelectPlayerObj.command = CommandEnum.UPGRADE_AR;
        PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_AR));
    }

    public void UpgradeHealthPoints()
    {
        SelectPlayerObj.command = CommandEnum.UPGRADE_HP;
        PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_HP));
    }

    public void UpgradeMoveRange()
    {
        SelectPlayerObj.command = CommandEnum.UPGRADE_MR;
        PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_MR));
    }

    public void UpgradeViewRange()
    {
        SelectPlayerObj.command = CommandEnum.UPGRADE_VR;
        PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_VR));
    }
}
