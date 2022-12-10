
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
        Debug.Log(obj.name);
        if (obj == null)
            return;
        obj.GetComponent<StatsAbstract>().upgradeVR();
    }


}
