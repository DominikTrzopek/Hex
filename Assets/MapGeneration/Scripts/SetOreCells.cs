using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOreCells : MonoBehaviour
{
    public static void SetCells(GameObject uiImage)
    {
        LayerMask layer = LayerMask.GetMask("Default");
        foreach (GameObject cell in HexGrid.hexArray)
        {
            if (cell.GetComponent<CustomTag>() != null)
            {
                if (cell.GetComponent<CustomTag>().HasTag(CellTag.ore))
                {
                    Collider[] neighbours = Physics.OverlapSphere(new Vector3(cell.transform.position.x, 0, cell.transform.position.z), HexMetrics.outerRadious + 0.1f, layer);
                    foreach (Collider neighbour in neighbours)
                    {
                        if (neighbour.GetComponent<CustomTag>() != null)
                        {
                            if (neighbour.GetComponent<CustomTag>().HasTag(CellTag.standard))
                            {
                                neighbour.GetComponent<CustomTag>().getResources = true;
                                neighbour.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                                GameObject ui = Object.Instantiate(uiImage, neighbour.transform);
                                ui.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }
}
