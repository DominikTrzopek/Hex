using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHPManager : MonoBehaviour
{
    void Update()
    {
        int hp = this.GetComponent<StatsAbstract>().GetHP();
        if(hp <= 0)
        {
            Vector2Int position = this.GetComponent<NetworkId>().position;
            CustomTag hexTag = HexGrid.hexArray[position.x, position.y].GetComponent<CustomTag>();
            hexTag.taken = false;
            hexTag.Rename(0, CellTag.standard);

            if(hexTag.getResources == true)
            {
                Resources.ChangeTmpIncome(-2);
            }

            Destroy(this.gameObject);
        }
    }
}
