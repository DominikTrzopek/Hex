
using UnityEngine;

public class UpgradeMRCommand : ICommand
{
    string ownerId;
    string objectId;

    public UpgradeMRCommand(CommandBuilder command)
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
        obj.GetComponent<UnitStats>().upgradeMR();
    }


}
