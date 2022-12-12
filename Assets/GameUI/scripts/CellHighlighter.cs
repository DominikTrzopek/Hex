using UnityEngine;

public class CellHighlighter : MonoBehaviour
{
    GameObject cellOutline;

    void OnMouseEnter()
    {
        Vector2Int position;
        NetworkId networkId = this.transform.root.GetComponent<NetworkId>();
        if (networkId == null)
            return;
        position = networkId.position;
        cellOutline = HexGrid.hexArray[position.x, position.y].transform.GetChild(1).gameObject;
        this.transform.root.Find("Canvas").gameObject.SetActive(true);
        if (cellOutline.transform.parent.GetComponent<CustomTag>().active == false)
        {
            cellOutline.SetActive(true);
        }
        cellOutline.GetComponent<SpriteRenderer>().color = PlayerColor.GetColor(networkId.ownerId);
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
