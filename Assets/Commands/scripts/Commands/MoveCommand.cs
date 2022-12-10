using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{

    string ownerId;
    string objectId;
    List<string> coordinateList;

    public MoveCommand(CommandBuilder command)
    {
        this.ownerId = command.ownerId;
        this.objectId = command.networkId;
        this.coordinateList = command.args;
    }

    public void Execute()
    {
        GameObject obj = FindNetworkObject.FindObj(objectId);
        if (obj == null)
            return;
        List<GameObject> path = ArgsParser.makePathFromCoordinates(coordinateList);
        obj.GetComponent<TankMovement>().startSelected = true;
        obj.GetComponent<TankMovement>().setPath(path);
        obj.GetComponent<TankMovement>().enabled = true;

        Vector2Int newPosition = path[path.Count - 1].GetComponent<CustomTag>().coordinates;
        path[path.Count - 1].GetComponent<CustomTag>().taken = true;
        path[0].GetComponent<CustomTag>().taken = false;
        obj.GetComponent<NetworkId>().position = newPosition;

        if(path[path.Count - 1].GetComponent<CustomTag>().getResources == true && obj.GetComponent<NetworkId>().ownerId == UDPServerConfig.getId())
            Resources.ChangeTmpIncome(1);

        
    }
}
