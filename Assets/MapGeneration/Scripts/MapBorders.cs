using UnityEngine;

public class MapBorders : MonoBehaviour
{
    public static void makeBorder(int size, GameObject border)
    {
        Vector3 scale = border.transform.localScale;
        float diff = scale.x / 2f;

        Vector3 start = HexGrid.hexArray[0, 0].transform.position;
        Vector3 end = HexGrid.hexArray[size - 1, size - 1].transform.position;
        //down
        Instantiate(border, new Vector3(start.x - diff, -0.99f, start.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        //up
        Instantiate(border, new Vector3(end.x + diff, -0.99f, end.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        //left
        Instantiate(border, new Vector3(start.x, -1, start.z - diff), Quaternion.Euler(new Vector3(0, 0, 0)));
        //right
        Instantiate(border, new Vector3(end.x, -1, end.z + diff), Quaternion.Euler(new Vector3(0, 0, 0)));

    }
}
