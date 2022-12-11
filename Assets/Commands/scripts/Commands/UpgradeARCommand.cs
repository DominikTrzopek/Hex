
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
        Debug.Log(obj.name);
        if (obj == null)
            return;
        obj.GetComponent<UnitStats>().upgradeAR();
        obj.GetComponent<StatsAbstract>().upgrade();
    }


}
