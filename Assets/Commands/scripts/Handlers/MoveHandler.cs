using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler : IActionHandler
{

    GameObject obj;
    List<GameObject> objInRange;

    public MoveHandler(GameObject obj)
    {
        this.obj = obj;
    }

    public void MakeAction()
    {
        if (obj.GetComponent<TankMovement>().moving == true)
        {
            Debug.Log("Unit is moving");
            return;
        }

        Vector2Int position = obj.GetComponent<NetworkId>().position;
        GameObject takenHex = HexGrid.hexArray[position.x, position.y];
        objInRange = PathFinding.SetRange(obj.GetComponent<UnitStats>().getMR(), takenHex);

        foreach (GameObject cell in objInRange)
        {
            cell.transform.GetChild(1).gameObject.SetActive(true);
            cell.GetComponent<CustomTag>().active = true;
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

    ~MoveHandler()
    {
        CancelAction();
    }
}
