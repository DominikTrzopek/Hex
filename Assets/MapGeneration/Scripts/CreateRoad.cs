using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoad
{
    public static float RoadRotation(GameObject n1, GameObject n2)
    {
        float rotation;
        Vector3 current;
        current = new Vector3(n2.transform.position.x - n1.transform.position.x, 0, n2.transform.position.z - n1.transform.position.z);
        rotation = HexMetrics.GetRotation(current);
        return rotation;
    }

    public static List<GameObject> Create(List<GameObject> road_path, GameObject road_str)
    {
        List<GameObject> roads = new List<GameObject>();
        for (int i = 0; i < road_path.Count - 1; i++)
        {
            float rotation = RoadRotation(road_path[i], road_path[i + 1]);
            Vector3 position = new Vector3(road_path[i].transform.position.x, 0.3f, road_path[i].transform.position.z);
            roads.Add(Object.Instantiate(road_str, position, Quaternion.Euler(new Vector3(0, rotation - 180, 0))));
        }
        return roads;
    }
}
