using System.Collections.Generic;
using UnityEngine;

public class GridManipulator : MonoBehaviour
{

    public static GameObject ReplaceTile(GameObject toDestroy, GameObject toSpawn, Color color, float yPosition = 0)
    {
        int rotation = Random.Range(0, 6);
        Vector3 position = new Vector3(toDestroy.transform.position.x, yPosition, toDestroy.transform.position.z);

        GameObject spawned = Instantiate(toSpawn, position, Quaternion.Euler(new Vector3(90, rotation * 60, 0)));

        Vector2Int coordinates = toDestroy.GetComponent<CustomTag>().coordinates;
        HexGrid.hexArray[coordinates.x, coordinates.y] = spawned;
        spawned.GetComponent<CustomTag>().coordinates = coordinates;
        spawned.GetComponent<Renderer>().material.color = color;

        Destroy(toDestroy);
        toDestroy.SetActive(false);
        return spawned;
    }

    public static bool PrepareGrid(int mapSize, int numOfPlayers, GameObject basePrefab, GameObject hex, GameObject ore, float yPosition, Biome biome, int border)
    {
        List<Vector2Int> spawnPoints = GetSpawnPoint(mapSize, numOfPlayers, border);
        List<GameObject> bases = SpawnBase(basePrefab, spawnPoints, yPosition, hex, biome);
        SpawnOre(ore, spawnPoints, biome, yPosition);
        bool validatation = ValidateMap(bases, yPosition);
        //setIds(bases);
        return validatation;
    }

    private static List<Vector2Int> GetSpawnPoint(int mapSize, int numOfPlayers, int border)
    {
        List<Vector2Int> spawnPoints = new List<Vector2Int>{
            new Vector2Int(mapSize - border, mapSize - border),
            new Vector2Int(border, border)
        };

        if (numOfPlayers > 2)
            spawnPoints.Add(new Vector2Int(mapSize - border, border));
        if (numOfPlayers > 3)
            spawnPoints.Add(new Vector2Int(border, mapSize - border));

        return spawnPoints;
    }

    private static void SpawnOre(GameObject ore, List<Vector2Int> baseSpawns, Biome biome, float yPosition = 0)
    {
        for (int i = 0; i < baseSpawns.Count; i++)
        {
            int offsetX = 2 * (int)Mathf.Pow(-1, i);
            int offsetY = i < 2 ? offsetX : -offsetX;
            ReplaceTile(HexGrid.hexArray[baseSpawns[i].x + offsetX, baseSpawns[i].y + offsetY], ore, biome.standardColor, yPosition);
        }
    }

    private static List<GameObject> SpawnBase(GameObject basePrefab, List<Vector2Int> spawnPoints, float yPosition, GameObject hex, Biome biome)
    {
        List<GameObject> spawned = new List<GameObject>();

        foreach (Vector2Int spawnPoint in spawnPoints)
        {
            GameObject toReplace = HexGrid.hexArray[spawnPoint.x, spawnPoint.y];
            spawned.Add(ReplaceTile(HexGrid.hexArray[spawnPoint.x, spawnPoint.y], basePrefab, Color.grey, yPosition));
        }
        ClearCellsAroundObjs(spawned, hex, biome.standardColor, yPosition);
        return spawned;
    }

    private static void ClearCellsAroundObjs(List<GameObject> targetObj, GameObject toReplace, Color color, float yPosition)
    {
        foreach (GameObject obj in targetObj)
        {
            LayerMask layer = LayerMask.GetMask("Default");
            Collider[] neighbours = Physics.OverlapSphere(new Vector3(obj.transform.position.x, 0, obj.transform.position.z), HexMetrics.outerRadious * 3, layer);

            foreach (Collider neighbour in neighbours)
            {
                if (!neighbour.gameObject.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
                    ReplaceTile(neighbour.gameObject, toReplace, color, yPosition);
            }
        }
    }

    private static bool ValidateMap(List<GameObject> bases, float yPosition)
    {
        List<GameObject> objects = PathFinding.SetRange(100, bases[0]);
        foreach (GameObject baseObj in bases)
        {
            if (baseObj.GetComponent<CustomTag>().pathTag != PathTag.inRange)
            {
                PathFinding.ClearDistance(objects);
                return false;
            }
        }
        PathFinding.ClearDistance(objects);
        return true;
    }

    private static void SetIds(List<GameObject> bases)
    {
        TCPConnection conn = TCPConnection.instance;
        for (int i = 0; i < bases.Count; i++)
        {
            bases[i].GetComponent<CustomTag>().Rename(0, CellTag.structure);
            bases[i].GetComponent<NetworkId>().setIds(conn.playerInfo[i].id, conn.playerInfo[i].id);
            bases[i].GetComponent<Renderer>().material.color = conn.playerInfo[i].color;
        }
    }


}
