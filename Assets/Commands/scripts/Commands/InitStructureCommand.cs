using System.Collections.Generic;
using UnityEngine;

public class InitStructureCommand : ICommand
{
    GameObject structure = PrefabContainer.container.structurePrefab;
    GameObject road = PrefabContainer.container.roadPrefab;
    string ownerId;
    string objectId;
    string creatorObjectId;
    List<string> coordinateList;

    public InitStructureCommand(CommandBuilder command)
    {
        this.ownerId = command.ownerId;
        this.objectId = command.networkId;
        this.creatorObjectId = command.args[0];
        command.args.RemoveAt(0);
        this.coordinateList = command.args;
    }

    public void Execute()
    {
        if(FindNetworkObject.FindObj(objectId) != null)
            return;

        GameObject creatorObj = FindNetworkObject.FindObj(creatorObjectId);
        if (creatorObj == null)
            return;

        List<GameObject> path = ArgsParser.MakePathFromCoordinates(coordinateList);
        GameObject endPath = path[path.Count - 1];
        Vector2Int endCoordinates = endPath.GetComponent<CustomTag>().coordinates;
        Vector3 structurePosition = HexGrid.hexArray[endCoordinates.x, endCoordinates.y].transform.position;

        GameObject newObj = Object.Instantiate(structure, structurePosition, Quaternion.Euler(new Vector3(0, Random.Range(0, 6) * 60, 0)));
        newObj.GetComponent<NetworkId>().position = endCoordinates;
        newObj.GetComponent<NetworkId>().SetIds(ownerId, objectId);
        newObj.GetComponent<StructureStats>().parentId = creatorObjectId;
        creatorObj.GetComponent<StructureStats>().AddConnected(newObj);
        newObj.GetComponent<TakeCell>().MarkCells(ownerId);

        CreateRoad.Create(path, road);

        CustomTag tags = endPath.GetComponent<CustomTag>();
        tags.Rename(0, CellTag.obstruction);
        if (tags.HasTag(CellTag.tree))
        {
            Object.Destroy(endPath.transform.GetChild(2).gameObject);
            tags.Rename(1, CellTag.structure);
        }
        else
            tags.Add(CellTag.structure);
    }
}
