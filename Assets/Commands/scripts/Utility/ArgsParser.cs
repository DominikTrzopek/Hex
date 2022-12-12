using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgsParser
{
    public static Vector2Int StringToVector2Int(string sVector)
    {
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        string[] sArray = sVector.Split(',');

        Vector2Int result = new Vector2Int(
           int.Parse(sArray[0]),
           int.Parse(sArray[1]));

        return result;
    }

    public static List<GameObject> MakePathFromCoordinates(List<string> coordinateList)
    {
        List<GameObject> path = new List<GameObject>();
        foreach (string stringCoordinates in coordinateList)
        {
            Vector2Int vector = ArgsParser.StringToVector2Int(stringCoordinates);
            path.Add(HexGrid.hexArray[vector.x, vector.y]);
        }
        return path;
    }
}
