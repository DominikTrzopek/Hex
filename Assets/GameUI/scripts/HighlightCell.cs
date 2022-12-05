using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCell : MonoBehaviour
{
    GameObject cellOutline;

    void OnMouseOver()
    {
        Vector2Int position = this.transform.parent.GetComponent<NetworkId>().position;
        cellOutline = HexGrid.hexArray[position.x, position.y].transform.GetChild(1).gameObject;
        cellOutline.SetActive(true);
        cellOutline.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    void OnMouseExit()
    {
        cellOutline.SetActive(false);
        cellOutline.GetComponent<SpriteRenderer>().color = Color.cyan;
    }
}
