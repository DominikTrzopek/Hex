using UnityEngine;

public class UpgradeHPCommand : ICommand
{
    string ownerId;
    string objectId;

    public UpgradeHPCommand(CommandBuilder command)
    {
        this.ownerId = command.ownerId;
        this.objectId = command.networkId;
    }

    public void Execute()
    {
        GameObject obj = FindNetworkObject.FindObj(objectId);
        if (obj == null)
            return;
        obj.GetComponent<StatsAbstract>().UpgradeHP();
        obj.GetComponent<StatsAbstract>().Upgrade();
    }
}
