using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCell : MonoBehaviour
{
    GameObject cellOutline;

    void OnMouseEnter()
    {
        Vector2Int position;
        if (this.transform.GetComponent<NetworkId>() != null)
            position = this.transform.GetComponent<NetworkId>().position;
        else
            position = this.transform.parent.GetComponent<NetworkId>().position;
        cellOutline = HexGrid.hexArray[position.x, position.y].transform.GetChild(1).gameObject;
        cellOutline.SetActive(true);
        cellOutline.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    void OnMouseExit()
    {
        cellOutline.SetActive(false);
        if (cellOutline.GetComponent<SelectCell>())
            cellOutline.GetComponent<SpriteRenderer>().color = cellOutline.GetComponent<SelectCell>().activeCellColor;
        else
            cellOutline.GetComponent<SpriteRenderer>().color = Color.cyan;
    }
}
