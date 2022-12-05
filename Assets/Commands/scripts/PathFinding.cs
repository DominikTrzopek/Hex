using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    //****************************************************************************************************************

    static public List<GameObject> SetRange(int range, GameObject start)
    {
        List<GameObject> objectsInRange = new List<GameObject>();
        List<GameObject> listOfCells = new List<GameObject>{start};
        LayerMask layer = LayerMask.GetMask("Default");
        int currentRange = 0;

        start.GetComponent<CustomTag>().range = currentRange;
        objectsInRange.Add(start);

        while (currentRange < range)
        {
            currentRange++;
            listOfCells = SetNext(listOfCells, currentRange, layer);
            if(listOfCells.Count == 0)
            {
                return objectsInRange;
            }
            objectsInRange.AddRange(listOfCells);
        }
        Debug.Log(objectsInRange.Count);
        return objectsInRange;
    }

    //****************************************************************************************************************

    static List<GameObject> SetNext(List<GameObject> listOfCells, int currentRange, LayerMask layer)
    {
        List<GameObject> nextCells = new List<GameObject>();
        for (int i = 0; i < listOfCells.Count; i++)
        {
            Collider[] neighbours = Physics.OverlapSphere(new Vector3(listOfCells[i].transform.position.x, 0, listOfCells[i].transform.position.z), HexMetrics.outerRadious + 0.1f, layer);
            nextCells.AddRange(CollidersToList(neighbours, currentRange));
        }
        return nextCells;
    }

    static List<GameObject> CollidersToList(Collider[] coll, int range)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < coll.Length; i++)
        {
            CustomTag tags = coll[i].gameObject.GetComponent<CustomTag>();
            if (tags.pathTag == PathTag.none && tags.HasTag(CellTag.standard) && tags.taken == false)
            {
                tags.pathTag = PathTag.inRange;
                tags.range = range;
                list.Add(coll[i].gameObject);
            }
        }
        return list;
    }

    //****************************************************************************************************************


    static GameObject FindNext(GameObject current_obj, int currentRange)
    {
        LayerMask layer = LayerMask.GetMask("Default");
        Collider[] neighbours = Physics.OverlapBox(new Vector3(current_obj.transform.position.x, 0, current_obj.transform.position.z), new Vector3(1, 10, 1), Quaternion.Euler(0, 0, 0), layer);
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i].GetComponent<CustomTag>().range == currentRange - 1)
            {
                current_obj = neighbours[i].gameObject;
                break;
            }
        }
        return current_obj;
    }

    //****************************************************************************************************************

    public static List<GameObject> FindPath(GameObject start, GameObject end)
    {
        //SetRange(range, start);
        List<GameObject> list = new List<GameObject>();
        GameObject pom;
        int obj_range = end.GetComponent<CustomTag>().range;
        if (obj_range == 0)
            return list;
        else
        {
            list.Insert(0, end);
            while (list.Count - 1 >= 0 && obj_range > 0)
            {
                pom = FindNext(list[0], obj_range);
                list.Insert(0, pom);
                obj_range--;
            }
            return list;
        }

    }

    //****************************************************************************************************************

    public static void ClearDistance(List<GameObject> objectsInRange)
    {
        if (objectsInRange != null)
        {
            foreach (GameObject obj in objectsInRange)
            {
                obj.GetComponent<CustomTag>().range = 1000;
                obj.GetComponent<CustomTag>().pathTag = PathTag.none;
                // if (obj.GetComponent<CustomTag>().in_base_range == false)
                //     obj.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            GameObject[] all = GameObject.FindGameObjectsWithTag("hex");
            foreach (GameObject obj in all)
            {
                obj.GetComponent<CustomTag>().range = 1000;
                obj.GetComponent<CustomTag>().pathTag = PathTag.none;
                // obj.transform.GetChild(0).gameObject.SetActive(false);
                // if (!obj.GetComponent<CustomTag>().HasTag(CellTag.structure))
                //     obj.GetComponent<CustomTag>().in_base_range = false;
            }
        }
    }

    //****************************************************************************************************************
}
