using UnityEngine;

public class UpgradeGunCommand : ICommand
{
    string ownerId;
    string objectId;

    public UpgradeGunCommand(CommandBuilder command)
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
        obj.GetComponent<UnitStats>().UpgradeAP();
        obj.GetComponent<UnitStats>().UpgradeAR();
        obj.GetComponent<StatsAbstract>().Upgrade();
    }
}
