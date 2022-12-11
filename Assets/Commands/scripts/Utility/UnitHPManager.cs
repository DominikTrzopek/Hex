using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHPManager : MonoBehaviour
{
    void Update()
    {
        int hp = this.GetComponent<StatsAbstract>().getHP();
        if(hp <= 0)
        {
            Vector2Int position = this.GetComponent<NetworkId>().position;
            HexGrid.hexArray[position.x, position.y].GetComponent<CustomTag>().taken = false;
            HexGrid.hexArray[position.x, position.y].GetComponent<CustomTag>().Rename(0, CellTag.standard);
            Destroy(this.gameObject);
        }
    }
}
