using UnityEngine;

public class CellHighlighter : MonoBehaviour
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
        this.transform.root.Find("Canvas").gameObject.SetActive(true);
        if (cellOutline.transform.parent.GetComponent<CustomTag>().active == false)
        {
            cellOutline.SetActive(true);
        }
        //TODO: ustawić kolor dla innych graczy
        cellOutline.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    void OnMouseExit()
    {
        this.transform.root.Find("Canvas").gameObject.SetActive(false);
        if (cellOutline.transform.parent.GetComponent<CustomTag>().active == false)
        {
            cellOutline.SetActive(false);
        }
        if (cellOutline.GetComponent<CellSelector>())
            cellOutline.GetComponent<SpriteRenderer>().color = cellOutline.GetComponent<CellSelector>().activeCellColor;
        else
            cellOutline.GetComponent<SpriteRenderer>().color = Color.cyan;
    }
}
