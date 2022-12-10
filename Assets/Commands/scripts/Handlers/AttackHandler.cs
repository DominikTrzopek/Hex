using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : IActionHandler
{
    GameObject unit;
    LayerMask layer;
    Color oldColor;
    Collider[] enemyObjects;
    List<Vector3> linePoints;
    List<GameObject> cells;
    LineController lineController;

    public AttackHandler(GameObject unit, LineController controller)
    {
        this.unit = unit;
        layer = LayerMask.GetMask("Player");
        cells = new List<GameObject>();
        linePoints = new List<Vector3>();
        lineController = controller;
    }

    public void MakeAction()
    {
        float range = unit.GetComponent<UnitStats>().getAR() * HexMetrics.outerRadious;
        enemyObjects = Physics.OverlapSphere(new Vector3(unit.transform.position.x, 0, unit.transform.position.z), range, layer);
        foreach (Collider enemy in enemyObjects)
        {
            NetworkId networkId = enemy.transform.parent.GetComponent<NetworkId>();
            if (networkId != null && networkId.ownerId != UDPServerConfig.getId())
            {
                Vector3 rayDirection = (enemy.transform.position - unit.transform.position).normalized * 10;
                rayDirection.y = 0.2f;
                Ray ray = new Ray(unit.transform.position, rayDirection);
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData))
                {
                    if (hitData.transform.gameObject == enemy.gameObject || hitData.transform.gameObject == enemy.transform.parent.gameObject)
                    {
                        GameObject cell = HexGrid.hexArray[networkId.position.x, networkId.position.y];
                        cell.GetComponent<CustomTag>().active = true;
                        cell.transform.GetChild(1).gameObject.SetActive(true);
                        cells.Add(cell);

                        oldColor = cell.GetComponent<SelectCell>().activeCellColor;
                        cell.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.red;
                        cell.GetComponent<SelectCell>().activeCellColor = Color.red;

                        Vector3 startPointTransform = new Vector3(unit.transform.position.x, .3f, unit.transform.position.z);

                        Vector3 endPointTransform = new Vector3(enemy.transform.position.x, .3f, enemy.transform.position.z);

                        linePoints.Add(startPointTransform);
                        linePoints.Add(endPointTransform);

                    }
                }
            }
        }
        lineController.SetUpLine(linePoints.ToArray());
    }

    public void CancelAction()
    {
        foreach (GameObject cell in cells)
        {
            cell.transform.GetChild(1).GetComponent<SpriteRenderer>().color = oldColor;
            cell.GetComponent<SelectCell>().activeCellColor = oldColor;
            cell.transform.GetChild(1).gameObject.SetActive(false);
            cell.GetComponent<CustomTag>().active = false;
            linePoints.Clear();
            lineController.SetUpLine(linePoints.ToArray());
        }
    }

    ~AttackHandler()
    {
        CancelAction();
    }
}
