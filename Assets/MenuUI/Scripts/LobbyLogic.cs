using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading;
using System;
using UnityEngine.SceneManagement;

public class LobbyLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject CellPrefab;
    [SerializeField]
    private GameObject deleteButton;
    [SerializeField]
    private LevelLoader levelLoader;
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
        if (UDPServerConfig.getSecretHash() != info.creatorId)
        {
            deleteButton.SetActive(false);
        }
        else
        {
            deleteButton.SetActive(true);
        }
        for (int i = 0; i < info.numberOfPlayers; i++)
        {
            GameObject newCell = Instantiate(CellPrefab);
            cells.Add(newCell);
            newCell.transform.SetParent(this.gameObject.transform, false);
        }
        isActive = true;
    }

    int tick = 0;

    void FixedUpdate()
    {
        if (isActive == true)
        {

            if (CheckIfPlayersReady())
            {
                tick++;
                if (tick >= 100)
                    StarGame();
            }
            else
                tick = 0;

            if (cells.Count > 0 && conn.messageQueue.Count > 0)
            {
                try
                {

                    ConnectMsg info = ConnectMsg.fromString(conn.messageQueue[0]);
                    if (info.playerInfo.id == UDPServerConfig.getId())
                        conn.selfNumber = info.playerInfo.number;
                    ErrorHandling.handle(info.code, this.transform.parent.parent.gameObject);
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
                catch (ArgumentException)
                {
                    //Debug.Log(err.ToString());
                    conn.messageQueue.RemoveAt(0);
                }
            }
        }
        if (isActive == true && !conn.client.socketReady && conn.messageQueue.Count <= 1)
        {
            ErrorHandling.handle(ResponseType.DISCONNECT, this.transform.parent.parent.gameObject);
            conn.clearConnection();
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

    public void StarGame()
    {
        conn = TCPConnection.instance;
        conn.client.writeSocket(new ConnectMsg(new PlayerInfo(PlayerStatus.INGAME, TCPConnection.instance.selfNumber)));
        foreach (GameObject cell in cells)
        {
            PlayersInfoLogic playerData = cell.GetComponent<PlayersInfoLogic>();
            playerData.playerInfo.color = playerData.image.color;
            conn.playerInfo.Add(playerData.playerInfo);
        }
        SceneManager.LoadScene(1);
    }

    bool CheckIfPlayersReady()
    {
        foreach (GameObject cell in cells)
        {
            PlayerStatus status = cell.GetComponent<PlayersInfoLogic>().playerInfo.status;
            if(status == PlayerStatus.INGAME)
                StarGame();
            if (status != PlayerStatus.READY)
                return false;    
        }
        return true;
    }


}
