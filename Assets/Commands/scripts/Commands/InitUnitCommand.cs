using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitUnitCommand : ICommand
{
    List<GameObject> units = PrefabContainer.container.playerUnitPrefab;
    string ownerId;
    string objectId;
    Vector2Int coordinates;
    float rotation;
    int prefabNum;

    public InitUnitCommand(CommandBuilder command)
    {
        this.ownerId = command.ownerId;
        this.objectId = command.networkId;
        this.coordinates.x = int.Parse(command.args[0]);
        this.coordinates.y = int.Parse(command.args[1]);
        this.rotation = float.Parse(command.args[2]);
        this.prefabNum = int.Parse(command.args[3]);
    }

    public void Execute()
    {
        if(FindNetworkObject.FindObj(objectId) != null)
            return;

        Vector3 position = HexGrid.hexArray[coordinates.x, coordinates.y].transform.position;
        GameObject newObj = Object.Instantiate(units[prefabNum], position, Quaternion.Euler(new Vector3(0, rotation, 0)));
        newObj.GetComponent<NetworkId>().position = coordinates;
        newObj.GetComponent<NetworkId>().setIds(ownerId, objectId);
        HexGrid.hexArray[coordinates.x, coordinates.y].GetComponent<CustomTag>().taken = true;

        Color color = PlayerInfoGetter.GetColor(ownerId);
        foreach (Transform child in newObj.GetComponentsInChildren<Transform>())
        {
            try
            {
                child.GetComponent<Renderer>().material.color = color;
            }
            catch { }
        }
    }
}
