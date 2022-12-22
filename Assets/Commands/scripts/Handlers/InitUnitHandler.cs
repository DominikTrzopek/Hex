using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitUnitHandler : IActionHandler
{

    GameObject obj;
    GameObject uiImage;
    private List<GameObject> cells = new List<GameObject>();
    private List<GameObject> icons = new List<GameObject>();


    public InitUnitHandler(GameObject obj, GameObject uiImage)
    {
        this.obj = obj;
        this.uiImage = uiImage;
    }

    public void MakeAction()
    {
        Vector2Int coordinates = obj.GetComponent<CustomTag>().coordinates;
        if (coordinates.y % 2 != 0)
        {
            AddToActiveList(HexGrid.hexArray[coordinates.x + 1, coordinates.y]);
            AddToActiveList(HexGrid.hexArray[coordinates.x, coordinates.y + 1]);
            AddToActiveList(HexGrid.hexArray[coordinates.x, coordinates.y - 1]);
        }
        else
        {
            AddToActiveList(HexGrid.hexArray[coordinates.x + 1, coordinates.y]);
            AddToActiveList(HexGrid.hexArray[coordinates.x - 1, coordinates.y + 1]);
            AddToActiveList(HexGrid.hexArray[coordinates.x - 1, coordinates.y - 1]);
        }

        foreach (GameObject cell in cells)
        {
            cell.transform.GetChild(1).gameObject.SetActive(true);
            cell.GetComponent<CustomTag>().active = true;
            cell.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            icons.Add(Object.Instantiate(uiImage, cell.transform));
        }
    }

    private void AddToActiveList(GameObject obj)
    {
        if (obj.GetComponent<CustomTag>().taken == false)
        {
            cells.Add(obj);
        }
    }

    public void CancelAction()
    {
        foreach (GameObject cell in cells)
        {
            cell.transform.GetChild(1).gameObject.SetActive(false);
            cell.GetComponent<CustomTag>().active = false;
            try
            {
                Object.Destroy(cell.transform.GetChild(2).gameObject);
            }
            catch { };

        }
        cells.Clear();
    }

    ~InitUnitHandler()
    {
        CancelAction();
    }
}
