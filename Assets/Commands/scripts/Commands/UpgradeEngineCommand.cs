using UnityEngine;

public class UpgradeEngineCommand : ICommand
{
    string ownerId;
    string objectId;

    public UpgradeEngineCommand(CommandBuilder command)
    {
        this.ownerId = command.ownerId;
        this.objectId = command.networkId;
    }

    public void Execute()
    {
        GameObject obj = FindNetworkObject.FindObj(objectId);
        Debug.Log(obj.name);
        if (obj == null)
            return;
        obj.GetComponent<UnitStats>().UpgradeMR();
        obj.GetComponent<UnitStats>().UpgradeVR();
        obj.GetComponent<StatsAbstract>().Upgrade();
    }
}
