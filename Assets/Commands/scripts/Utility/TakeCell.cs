using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCell : MonoBehaviour
{
    public int range;
    public List<Collider> takenCells;
    public void MarkCells(string ownerId)
    {
        takenCells = new List<Collider>(takenCells);
        LayerMask layer = LayerMask.GetMask("Default");
        Vector3 center = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Color playerColor = PlayerInfoGetter.GetColor(this.transform.root.GetComponent<NetworkId>().ownerId);
        playerColor.a = ColorList.alpha;

        Collider[] cells = Physics.OverlapSphere(center, range * HexMetrics.outerRadious, layer);
        foreach (Collider coll in cells)
        {
            CustomTag tags = coll.GetComponent<CustomTag>();
            if ((tags.HasTag(CellTag.standard) || tags.HasTag(CellTag.obstruction)) && tags.takenBy.Trim() == "")
            {
                takenCells.Add(coll);
                tags.takenBy = ownerId;
                coll.transform.GetChild(0).GetComponent<SpriteRenderer>().color = playerColor;
            }
        }
    }

    void OnDestroy()
    {
        StructureStats structure = this.GetComponent<StructureStats>();

        Color destroyedColor = Color.grey;
        destroyedColor.a = 0.1f;
        foreach (Collider coll in takenCells)
        {
            if (coll != null)
            {
                coll.GetComponent<CustomTag>().takenBy = "";
                coll.transform.GetChild(0).GetComponent<SpriteRenderer>().color = destroyedColor;
            }
        }

        structure.DestroyAllConnected();

    }
}
