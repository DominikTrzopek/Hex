
using UnityEngine;

public class UpgradeAPCommand : ICommand
{
    string ownerId;
    string objectId;

    public UpgradeAPCommand(CommandBuilder command)
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
        obj.GetComponent<UnitStats>().upgradeAP();
    }


}
