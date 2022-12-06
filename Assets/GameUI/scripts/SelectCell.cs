using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCell : MonoBehaviour
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
