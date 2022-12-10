
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
        if (obj == null)
            return;
        obj.GetComponent<StructureStats>().upgrade();
    }


}
