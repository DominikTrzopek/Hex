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
        replaceTile(HexGrid.hex_array[baseSpawns[0].x + 2, baseSpawns[0].y + 2], ore, biome.standardColor, yPosition);
        replaceTile(HexGrid.hex_array[baseSpawns[1].x - 2, baseSpawns[1].y - 2], ore, biome.standardColor, yPosition);
        if(numOfPlayers > 2)
            replaceTile(HexGrid.hex_array[baseSpawns[2].x + 2, baseSpawns[2].y - 2], ore, biome.standardColor, yPosition);
        if(numOfPlayers > 3)
            replaceTile(HexGrid.hex_array[baseSpawns[3].x - 2, baseSpawns[3].y + 2], ore, biome.standardColor, yPosition);
    }

    public static GameObject replaceTile(GameObject toDestroy, GameObject toSpawn, Color color,  float yPosition = 0)
    {
        int rotation = Random.Range(0, 6);
        Vector3 position = new Vector3(toDestroy.transform.position.x, yPosition , toDestroy.transform.position.z);
        GameObject spawned = Instantiate(toSpawn, position, Quaternion.Euler(new Vector3(90, rotation * 60, 0)));
        spawned.GetComponent<Renderer>().material.color = color;
        Vector2Int coordinates = toDestroy.GetComponent<CustomTag>().coordinates;
        HexGrid.hex_array[coordinates.x, coordinates.y] = spawned;
        Destroy(toDestroy);
        toDestroy.SetActive(false);
        return spawned;
    }

    public static List<GameObject> spawnBase(GameObject basePrefab, List<Vector2Int> spawnPoints,  float yPosition, GameObject hex, Biome biome)
    {
        List<GameObject> spawned = new List<GameObject>();
        
        foreach(Vector2Int spawnPoint in spawnPoints)
        {
            GameObject toReplace = HexGrid.hex_array[spawnPoint.x, spawnPoint.y];
            spawned.Add(replaceTile(HexGrid.hex_array[spawnPoint.x, spawnPoint.y], basePrefab, Color.grey ,  yPosition));
        }

        foreach(GameObject baseObj in spawned)
        {

            LayerMask layer = LayerMask.GetMask("Default");
            Collider[] neighbours = Physics.OverlapSphere(new Vector3(baseObj.transform.position.x, 0, baseObj.transform.position.z), HexMetrics.outerRadious * 3, layer);
            Debug.Log(neighbours.Length);

            foreach(Collider neighbour in neighbours)
            {
                if(!neighbour.gameObject.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
                    replaceTile(neighbour.gameObject, hex, biome.standardColor, yPosition);
                else
                    Debug.Log(neighbour.transform.position);
            }

        }
        return spawned;
    }

    public static bool prepareGrid(int mapSize, int numOfPlayers, GameObject basePrefab, GameObject hex, GameObject ore, float yPosition , Biome biome,  float padding = 0.2f)
    {
        List<Vector2Int> spawnPoints = getSpawnPoint(mapSize, numOfPlayers, padding);
        List<GameObject> bases = spawnBase(basePrefab, spawnPoints, yPosition, hex, biome);
        spawnOre(ore, spawnPoints, biome, yPosition);
        return validateMap(bases, yPosition);

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

    //set tag and id for bases


}
