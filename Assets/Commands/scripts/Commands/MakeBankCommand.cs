
using UnityEngine;

public class MakeBankCommand : ICommand
{
    string ownerId;
    string objectId;

    public MakeBankCommand(CommandBuilder command)
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
        obj.GetComponent<StructureStats>().upgrade();
    }


}
