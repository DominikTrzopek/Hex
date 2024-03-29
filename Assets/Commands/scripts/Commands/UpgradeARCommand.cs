using UnityEngine;

public class UpgradeARCommand : ICommand
{
    string ownerId;
    string objectId;

    public UpgradeARCommand(CommandBuilder command)
    {
        this.ownerId = command.ownerId;
        this.objectId = command.networkId;
    }

    public void Execute()
    {
        GameObject obj = FindNetworkObject.FindObj(objectId);
        if (obj == null)
            return;
        obj.GetComponent<UnitStats>().UpgradeAR();
        obj.GetComponent<StatsAbstract>().Upgrade();
    }
}
