using System.Collections;
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

        GameObject creatorObj = FindNetworkObject.FindObj(creatorObjectId);
        if (creatorObj == null)
            return;

        List<GameObject> path = ArgsParser.makePathFromCoordinates(coordinateList);
        GameObject endPath = path[path.Count - 1];
        Vector2Int endCoordinates = endPath.GetComponent<CustomTag>().coordinates;
        Vector3 structurePosition = HexGrid.hexArray[endCoordinates.x, endCoordinates.y].transform.position;
        GameObject newObj = Object.Instantiate(structure, structurePosition, Quaternion.Euler(new Vector3(0, Random.Range(0, 6) * 60, 0)));
        newObj.GetComponent<NetworkId>().position = endCoordinates;
        newObj.GetComponent<NetworkId>().setIds(ownerId, objectId);
        CreateRoad.Create(path, road);
        endPath.GetComponent<CustomTag>().Rename(0, CellTag.obstruction);
        if (endPath.GetComponent<CustomTag>().HasTag(CellTag.tree))
        {
            Object.Destroy(endPath.transform.GetChild(2).gameObject);
            endPath.GetComponent<CustomTag>().Rename(1, CellTag.structure);
        }
        else
            endPath.GetComponent<CustomTag>().Add(CellTag.structure);
        creatorObj.GetComponent<StructureStats>().addConnected(newObj);

    }

    // public void Execute()
    // {
    //     List<GameObject> path = ArgsParser.makePathFromCoordinates(coordinateList);
    //     GameObject endPath = path[path.Count - 1];
    //     Vector2Int endCoordinates = endPath.GetComponent<CustomTag>().coordinates;
    //     Vector3 structurePosition = HexGrid.hexArray[endCoordinates.x, endCoordinates.y].transform.position;
    //     GameObject newObj = GridManipulator.ReplaceTile(endPath, structure, endPath.GetComponent<Renderer>().material.color, HexGrid.fixedYPosition);
    //     newObj.GetComponent<NetworkId>().position = endCoordinates;
    //     newObj.GetComponent<NetworkId>().setIds(ownerId, objectId);
    //     CreateRoad.Create(path, road);
    // }
}
