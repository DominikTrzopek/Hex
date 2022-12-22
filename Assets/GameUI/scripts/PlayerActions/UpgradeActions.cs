using UnityEngine;

public class UpgradeActions : ActionsAbstract, IActions
{
    public static UpgradeActions instance { get; private set; }

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
        if (CheckResourceRequirements(Costs.container.upgradeGun))
        {
            PlayerActionSelector.command = CommandEnum.UPGRADE_GUN;
            PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_GUN));
        }
    }

    public void UpgradeUnitHealthPoints()
    {
        if (CheckResourceRequirements(Costs.container.upgradeChasis))
        {
            PlayerActionSelector.command = CommandEnum.UPGRADE_CHASIS;
            PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_CHASIS));
        }
    }

    public void UpgradeStructureHealthPoints()
    {
        if (CheckResourceRequirements(Costs.container.upgradeStructure))
        {
            PlayerActionSelector.command = CommandEnum.UPGRADE_STRUCTURE;
            PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_STRUCTURE));
        }
    }

    public void UpgradeUnitMovement()
    {
        if (CheckResourceRequirements(Costs.container.upgradeEngine))
        {
            PlayerActionSelector.command = CommandEnum.UPGRADE_ENGINE;
            PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_ENGINE));
        }
    }

    public void UpgradeStructureViewRange()
    {
        if (CheckResourceRequirements(Costs.container.upgradeRadio))
        {
            PlayerActionSelector.command = CommandEnum.UPGRADE_RADIO;
            PerformAction(new UpgradeHandler(obj, CommandEnum.UPGRADE_RADIO));
        }
    }
}
