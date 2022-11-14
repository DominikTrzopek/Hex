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

    void Awake()
    {
        isActive = true;
        conn = TCPConnection.instance;
        new Thread(() =>
        {
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
                        }
                        catch (ArgumentException)
                        {
                            //Debug.Log("End of message");
                        }
                    }
                }
            }
        }).Start();
    }

    void Update()
    {
        if (cells.Count > 0 && info_FIFO.Count > 0)
        {
            Debug.Log(info_FIFO[0].id);
            int foundIndex = FindCell(info_FIFO[0].id);
            Debug.Log(foundIndex);
            if (foundIndex != -1)
                cells[foundIndex].GetComponent<PlayersInfoLogic>().playerInfo = info_FIFO[0];
            info_FIFO.RemoveAt(0);
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
        foreach(GameObject cell in cells)
        {
            conn.playerInfo.Add(cell.GetComponent<PlayersInfoLogic>().playerInfo);
        }
    }

    void OnEnable()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        TCPConnection conn = TCPConnection.instance;
        TCPServerInfo info = conn.serverInfo;
        for (int i = 0; i < info.numberOfPlayers; i++)
        {
            GameObject newCell = Instantiate(CellPrefab);
            cells.Add(newCell);
            newCell.transform.SetParent(this.gameObject.transform, false);
        }
    }
}
