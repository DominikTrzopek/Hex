using UnityEngine;

public class UpgradeRadioCommand : ICommand
{
    string ownerId;
    string objectId;

    public UpgradeRadioCommand(CommandBuilder command)
    {
        this.ownerId = command.ownerId;
        this.objectId = command.networkId;
    }

    public void Execute()
    {
        GameObject obj = FindNetworkObject.FindObj(objectId);
        if (obj == null)
            return;
        obj.GetComponent<StatsAbstract>().UpgradeVR();
        obj.GetComponent<StatsAbstract>().Upgrade();
    }
}
