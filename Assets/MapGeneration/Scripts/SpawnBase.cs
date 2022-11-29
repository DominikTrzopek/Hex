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

    public static GameObject replaceTile(GameObject toDestroy, GameObject toSpawn)
    {
        int rotation = Random.Range(0, 6);
        GameObject spawned = Instantiate(toSpawn, toDestroy.transform.position, Quaternion.Euler(new Vector3(90, rotation * 60, 0)));
        spawned.transform.position = toDestroy.transform.position;
        Destroy(toDestroy);
        return spawned;
    }

    public static List<GameObject> spawnBase(GameObject basePrefab, List<Vector2Int> spawnPoints)
    {
        List<GameObject> spawned = new List<GameObject>();
        foreach(Vector2Int spawnPoint in spawnPoints)
        {
            GameObject toReplace = HexGrid.hex_array[spawnPoint.x, spawnPoint.y];
            spawned.Add(replaceTile(toReplace, basePrefab));
        }
        return spawned;
    }

    public static void prepareGrid(int mapSize, int numOfPlayers, GameObject basePrefab, float padding = 0.2f)
    {
        List<Vector2Int> spawnPoints = getSpawnPoint(mapSize, numOfPlayers, padding);
        List<GameObject> bases = spawnBase(basePrefab, spawnPoints);
        Debug.Log(validateMap(bases));

    }

    public static bool validateMap(List<GameObject> bases)
    {
        List<GameObject> objects = PathFinding.SetRange(100, bases[0]);
        Debug.Log(objects.Count);
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


}
