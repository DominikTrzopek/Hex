using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeActions : ActionsAbstract, IPlayerObjectHandler
{
    public static UpgradeActions instance { get; private set; }
    GameObject obj;

    public GameObject tooltip;
    public TMPro.TextMeshProUGUI textMeshPro;

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

    public void UpgradeUnitAttack()
    {
        if (CheckRequirements(Costs.container.upgradeGun))
        {
            SelectPlayerObj.command = CommandEnum.UPGRADE_GUN;
            PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_GUN));
        }
        else
            textMeshPro.text = "Not enough resources!";
    }

    public void UpgradeUnitHealthPoints()
    {
        if (CheckRequirements(Costs.container.upgradeChasis))
        {
            SelectPlayerObj.command = CommandEnum.UPGRADE_CHASIS;
            PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_CHASIS));
        }
        else
            textMeshPro.text = "Not enough resources!";
    }

    public void UpgradeStructureHealthPoints()
    {
        if (CheckRequirements(Costs.container.upgradeStructure))
        {
            SelectPlayerObj.command = CommandEnum.UPGRADE_STRUCTURE;
            PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_STRUCTURE));
        }
        else
            textMeshPro.text = "Not enough resources!";
    }

    public void UpgradeUnitMovement()
    {
        if (CheckRequirements(Costs.container.upgradeEngine))
        {
            SelectPlayerObj.command = CommandEnum.UPGRADE_ENGINE;
            PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_ENGINE));
        }
        else
            textMeshPro.text = "Not enough resources!";
    }

    public void UpgradeStructureViewRange()
    {
        if (CheckRequirements(Costs.container.upgradeRadio))
        {
            SelectPlayerObj.command = CommandEnum.UPGRADE_RADIO;
            PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_RADIO));
        }
        else
            textMeshPro.text = "Not enough resources!";
    }
}
