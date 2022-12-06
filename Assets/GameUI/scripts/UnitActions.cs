using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActions : MonoBehaviour
{
    public static UnitActions instance { get; private set; }
    public GameObject obj;
    public GameObject takenHex;
    private List<GameObject> objInRange;

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

        if(obj.GetComponent<TankMovement>().moving == true)
        {
            Debug.Log("Unit is moveing");
            return;
        }
        SelectPlayerObj.command = CommandEnum.MOVE;

        Vector2Int position = obj.GetComponent<NetworkId>().position;
        GameObject takenHex = HexGrid.hexArray[position.x, position.y];
        objInRange = PathFinding.SetRange(5, takenHex);

        foreach(GameObject cell in objInRange)
        {
            cell.transform.GetChild(1).gameObject.SetActive(true);
            cell.GetComponent<CustomTag>().active = true;
        }
    }

    public void CancelAction()
    {
        PathFinding.ClearDistance(objInRange);

        foreach(GameObject cell in objInRange)
        {
            cell.transform.GetChild(1).gameObject.SetActive(false);
            cell.GetComponent<CustomTag>().active = false;
        }
    }
}
