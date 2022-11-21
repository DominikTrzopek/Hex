using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading;
using System;

public class LobbyLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject CellPrefab;
    private TCPConnection conn;
    private bool isActive = false;
    private List<PlayerInfo> info_FIFO = new List<PlayerInfo>();
    private List<GameObject> cells = new List<GameObject>();

    void OnEnable()
    {

        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        cells.Clear();
        TCPConnection conn = TCPConnection.instance;
        TCPServerInfo info = conn.serverInfo;
        for (int i = 0; i < info.numberOfPlayers; i++)
        {
            GameObject newCell = Instantiate(CellPrefab);
            cells.Add(newCell);
            newCell.transform.SetParent(this.gameObject.transform, false);
        }
        isActive = true;
        conn = TCPConnection.instance;
        
        new Thread(() =>
        {
            Debug.Log(isActive);
            while (conn.client.socketReady && isActive)
            {
                byte[] bytes = conn.client.readSocket();
                if (bytes != null)
                {
                    string message = Encoding.Default.GetString(bytes);
                    string[] splieted = message.Split("\n");
                    foreach (string part in splieted)
                    {
                        try
                        {
                            ConnectMsg response = ConnectMsg.fromString(part);
                            info_FIFO.Add(response.playerInfo);
                            Debug.Log(part);
                        }
                        catch (ArgumentException)
                        {
                            Debug.Log(part);
                        }
                    }
                }
            }
        }).Start();
    }

    void Update()
    {
        if(isActive)
        {
            if (cells.Count > 0 && info_FIFO.Count > 0)
            {
                int foundIndex = FindCell(info_FIFO[0].id);
                if (foundIndex != -1)
                {
                    cells[foundIndex].GetComponent<PlayersInfoLogic>().playerInfo = info_FIFO[0];
                    if (info_FIFO[0].status == PlayerStatus.NOTCONNECTED)
                    {
                        cells[foundIndex].GetComponent<PlayersInfoLogic>().playerInfo.id = null;
                    }
                }
                info_FIFO.RemoveAt(0);
            }
        }
    }

    private int FindCell(string id)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].GetComponent<PlayersInfoLogic>().playerInfo.id == id)
                return i;

        }
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].GetComponent<PlayersInfoLogic>().playerInfo.id == null && id != null && id.Trim() != "")
                return i;
        }
        return -1;
    }

    void OnDisable()
    {
        isActive = false;
    }

    void OnDestroy()
    {
        isActive = false;
    }

    void StarGame()
    {
        conn = TCPConnection.instance;
        foreach (GameObject cell in cells)
        {
            PlayersInfoLogic playerData = cell.GetComponent<PlayersInfoLogic>();
            playerData.playerInfo.color = playerData.image.color;
            conn.playerInfo.Add(playerData.playerInfo);
        }
    }

  
}
