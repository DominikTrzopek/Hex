using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject CellPrefab;

    void OnEnable(){

        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        TCPConnection conn = TCPConnection.instance;
        TCPServerInfo info = conn.serverInfo;
        for(int i = 0; i < info.numberOfPlayers; i++){
            GameObject newCell = Instantiate(CellPrefab);
            newCell.transform.SetParent(this.gameObject.transform, false);
        }
    }
}
