using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitStructureHandler : IActionHandler
{
    GameObject obj;
    List<GameObject> objInRange;

    public InitStructureHandler(GameObject obj)
    {
        this.obj = obj;
    }

    public void MakeAction()
    {
        Vector2Int position = obj.GetComponent<NetworkId>().position;
        GameObject takenHex = HexGrid.hexArray[position.x, position.y];
        objInRange = PathFinding.SetRange(obj.GetComponent<StructureStats>().GetTakenRange(), takenHex);

        foreach (GameObject cell in objInRange)
        {
            if(cell.GetComponent<CustomTag>().range > 1)
            {
                cell.transform.GetChild(1).gameObject.SetActive(true);
                cell.GetComponent<CustomTag>().active = true;
            }
        }
    }

    public void CancelAction()
    {
        PathFinding.ClearDistance(objInRange);

        foreach (GameObject cell in objInRange)
        {
            if(cell != null)
            {
                cell.transform.GetChild(1).gameObject.SetActive(false);
                cell.GetComponent<CustomTag>().active = false;
            }
        }
    }

    ~InitStructureHandler()
    {
        CancelAction();
    }
}
