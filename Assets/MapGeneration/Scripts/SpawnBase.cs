using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnBase : MonoBehaviour
{
    public static List<Vector2Int> getSpawnPoint(int mapSize, int numOfPlayers, float padding)
    {
        List<Vector2Int> spawnPoints = new List<Vector2Int>();
        int border = (int)(mapSize * padding);
        int spawnCord = mapSize - border;
        spawnPoints.Add(new Vector2Int(mapSize - border, mapSize - border));
        spawnPoints.Add(new Vector2Int(border, border));
        if(numOfPlayers > 2)
            spawnPoints.Add(new Vector2Int(mapSize - border, border));
        if(numOfPlayers > 3)
            spawnPoints.Add(new Vector2Int(border, mapSize - border));
        return spawnPoints;
    }

    public static void spawnOre(GameObject ore, List<Vector2Int> baseSpawns, Biome biome, float yPosition = 0)
    {
        int numOfPlayers = baseSpawns.Count;
        replaceTile(HexGrid.hexArray[baseSpawns[0].x + 2, baseSpawns[0].y + 2], ore, biome.standardColor, yPosition);
        replaceTile(HexGrid.hexArray[baseSpawns[1].x - 2, baseSpawns[1].y - 2], ore, biome.standardColor, yPosition);
        if(numOfPlayers > 2)
            replaceTile(HexGrid.hexArray[baseSpawns[2].x + 2, baseSpawns[2].y - 2], ore, biome.standardColor, yPosition);
        if(numOfPlayers > 3)
            replaceTile(HexGrid.hexArray[baseSpawns[3].x - 2, baseSpawns[3].y + 2], ore, biome.standardColor, yPosition);
    }

    public static GameObject replaceTile(GameObject toDestroy, GameObject toSpawn, Color color,  float yPosition = 0)
    {
        int rotation = Random.Range(0, 6);
        Vector3 position = new Vector3(toDestroy.transform.position.x, yPosition , toDestroy.transform.position.z);
        GameObject spawned = Instantiate(toSpawn, position, Quaternion.Euler(new Vector3(90, rotation * 60, 0)));
        spawned.GetComponent<Renderer>().material.color = color;
        Vector2Int coordinates = toDestroy.GetComponent<CustomTag>().coordinates;
        spawned.GetComponent<CustomTag>().coordinates = coordinates;
        HexGrid.hexArray[coordinates.x, coordinates.y] = spawned;
        Destroy(toDestroy);
        toDestroy.SetActive(false);
        return spawned;
    }

    public static List<GameObject> spawnBase(GameObject basePrefab, List<Vector2Int> spawnPoints,  float yPosition, GameObject hex, Biome biome)
    {
        List<GameObject> spawned = new List<GameObject>();
        
        foreach(Vector2Int spawnPoint in spawnPoints)
        {
            GameObject toReplace = HexGrid.hexArray[spawnPoint.x, spawnPoint.y];
            spawned.Add(replaceTile(HexGrid.hexArray[spawnPoint.x, spawnPoint.y], basePrefab, Color.grey ,  yPosition));
        }

        foreach(GameObject baseObj in spawned)
        {

            LayerMask layer = LayerMask.GetMask("Default");
            Collider[] neighbours = Physics.OverlapSphere(new Vector3(baseObj.transform.position.x, 0, baseObj.transform.position.z), HexMetrics.outerRadious * 3, layer);

            foreach(Collider neighbour in neighbours)
            {
                if(!neighbour.gameObject.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
                    replaceTile(neighbour.gameObject, hex, biome.standardColor, yPosition);
            }

        }
        return spawned;
    }

    public static bool prepareGrid(int mapSize, int numOfPlayers, GameObject basePrefab, GameObject hex, GameObject ore, float yPosition , Biome biome,  float padding = 0.2f)
    {
        List<Vector2Int> spawnPoints = getSpawnPoint(mapSize, numOfPlayers, padding);
        List<GameObject> bases = spawnBase(basePrefab, spawnPoints, yPosition, hex, biome);
        spawnOre(ore, spawnPoints, biome, yPosition);
        bool validatation = validateMap(bases, yPosition);
        //setIds(bases);
        return validatation;
    }

    public static bool validateMap(List<GameObject> bases , float yPosition)
    {
        List<GameObject> objects = PathFinding.SetRange(100, bases[0]);
        foreach(GameObject baseObj in bases)
        {
            if(baseObj.GetComponent<CustomTag>().pathTag != PathTag.inRange)
            {
                PathFinding.ClearDistance(objects);
                return false;
            }
        }
        PathFinding.ClearDistance(objects);
        return true;
    }

    public static void setIds(List<GameObject> bases)
    {
        TCPConnection conn = TCPConnection.instance;
        for(int i = 0; i < bases.Count; i++)
        {
            bases[i].GetComponent<CustomTag>().Rename(0, CellTag.structure);
            bases[i].GetComponent<NetworkId>().setIds(conn.playerInfo[i].id, conn.playerInfo[i].id);
            bases[i].GetComponent<Renderer>().material.color = conn.playerInfo[i].color;
        }
    }


}
