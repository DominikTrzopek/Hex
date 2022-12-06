using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActions : MonoBehaviour, IActionsHandler
{
    public static UnitActions instance { get; private set; }
    GameObject obj;
    GameObject takenHex;


    private List<GameObject> objInRange;

    public void setObj(GameObject toSet)
    {
        obj = toSet;
    }

    public GameObject getTakenHex()
    {
        return takenHex;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void MoveUnit()
    {

        if (obj.GetComponent<TankMovement>().moving == true)
        {
            Debug.Log("Unit is moving");
            return;
        }
        SelectPlayerObj.command = CommandEnum.MOVE;

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
            cell.transform.GetChild(1).gameObject.SetActive(false);
            cell.GetComponent<CustomTag>().active = false;
        }
    }
}
