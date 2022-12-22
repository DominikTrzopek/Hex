using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler : IActionHandler
{

    GameObject obj;
    List<GameObject> objInRange;
    Color oldColor;

    public MoveHandler(GameObject obj)
    {
        this.obj = obj;
    }

    public void MakeAction()
    {
        Vector2Int position = obj.GetComponent<NetworkId>().position;
        GameObject takenHex = HexGrid.hexArray[position.x, position.y];
        PathFinding.ClearDistance(null);
        objInRange = PathFinding.SetRange(obj.GetComponent<UnitStats>().GetMR(), takenHex);

        foreach (GameObject cell in objInRange)
        {
            if(cell.GetComponent<CustomTag>().range != 0)
            {
                cell.transform.GetChild(1).gameObject.SetActive(true);
                cell.GetComponent<CustomTag>().active = true;

                if(cell.transform.Find("shovelIcon(Clone)"))
                {
                    cell.transform.GetChild(cell.transform.childCount - 1).gameObject.SetActive(true);
                    oldColor = cell.GetComponent<CellSelector>().activeCellColor;
                    cell.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.yellow;
                    cell.GetComponent<CellSelector>().activeCellColor = Color.yellow;
                }
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

                if(cell.transform.Find("shovelIcon(Clone)"))
                {
                    cell.transform.GetChild(cell.transform.childCount - 1).gameObject.SetActive(false);
                    cell.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.yellow;
                    cell.GetComponent<CellSelector>().activeCellColor = oldColor;
                    cell.transform.GetChild(1).GetComponent<SpriteRenderer>().color = oldColor;
                }
            }
        }

    }

    ~MoveHandler()
    {
        CancelAction();
    }
}
