using UnityEngine;

public class CellSelector : MonoBehaviour
{

    public Color activeCellColor;
    public Color selectedCellColor;

    void OnMouseOver()
    {
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = selectedCellColor;
    }

    void OnMouseExit()
    {
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = activeCellColor;
    }

}
