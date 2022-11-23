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
    [SerializeField]
    private GameObject errorScreen;
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
        conn = TCPConnection.instance;
        TCPServerInfo info = conn.serverInfo;
        for (int i = 0; i < info.numberOfPlayers; i++)
        {
            GameObject newCell = Instantiate(CellPrefab);
            cells.Add(newCell);
            newCell.transform.SetParent(this.gameObject.transform, false);
        }
        Debug.Log(conn.messageQueue.Count);
        isActive = true;
    }

    void Update()
    {
        if(isActive == true && conn.client.socketReady)
        {
            if (cells.Count > 0 && conn.messageQueue.Count > 0)
            {
                try 
                {
                    
                    ConnectMsg info = ConnectMsg.fromString(conn.messageQueue[0]);
                    if(info.code == ResponseType.WRONGPASSWORD)
                    {
                        errorScreen.SetActive(true);
                        errorScreen.transform.Find("ErrorMessage").GetComponent<TMPro.TextMeshProUGUI>().text = "Authorization failed, wrong password!";
                        Debug.Log(this.transform.parent.parent.gameObject.name);
                        this.transform.parent.parent.gameObject.SetActive(false);
                    }
                    if(info.code == ResponseType.BADREQUEST || info.code == ResponseType.BADARGUMENTS)
                    {
                        errorScreen.SetActive(true);
                        errorScreen.transform.Find("ErrorMessage").GetComponent<TMPro.TextMeshProUGUI>().text = "Authorization failed, bad message structure!";
                        Debug.Log(this.transform.parent.parent.gameObject.name);
                        this.transform.parent.parent.gameObject.SetActive(false);
                    }
                    int foundIndex = FindCell(info.playerInfo.id);
                    if (foundIndex != -1)
                    {
                        cells[foundIndex].GetComponent<PlayersInfoLogic>().playerInfo = info.playerInfo;
                        if (info.playerInfo.status == PlayerStatus.NOTCONNECTED)
                        {
                            cells[foundIndex].GetComponent<PlayersInfoLogic>().playerInfo.id = null;
                        }
                    }
                    conn.messageQueue.RemoveAt(0);
                }
                catch (ArgumentException err)
                {
                    Debug.Log(err.ToString());
                    conn.messageQueue.RemoveAt(0);
                }
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
