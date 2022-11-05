using UnityEngine;
using System.Text;
using System.Threading;
using System;
using System.Collections.Generic;

public class ServerListScreenLogic : MonoBehaviour
{

    [SerializeField]
    private GameObject CellPrefab;

    private static Mutex mutex = new Mutex();

    void OnEnable()
    {
        List<TCPServerInfo> tcpServers = new List<TCPServerInfo>();
        //RequestServerList
        Thread thread = new Thread(() =>
        {
            mutex.WaitOne();
            UDPClient client = new UDPClient();
            client.init();
            client.sendData(new GetServerListRequest(1));
            while (true)
            {
                try
                {
                    byte[] byteResponse = client.receiveData();
                    string message = Encoding.Default.GetString(byteResponse);
                    UDPResponse udpResponse = UDPResponse.fromString(message);
                    if (udpResponse.responseType == ResponseType.ENDOFMESSAGE)
                    {
                        mutex.ReleaseMutex();
                        return;
                    }

                    tcpServers.Add(udpResponse.serverInfo);

                }
                catch (Exception err)
                {
                    Debug.Log(err.ToString());
                    mutex.ReleaseMutex();
                    return;
                }
            }
        });

        thread.Start();
        thread.Join();
        DisplayServerInfo(tcpServers);

    }

    private void DisplayServerInfo(List<TCPServerInfo> tcpServers)
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (var serverInfo in tcpServers)
        {
            GameObject newCell = Instantiate(CellPrefab);
            newCell.transform.SetParent(this.gameObject.transform, false);

            TMPro.TextMeshProUGUI serverNameText = newCell.transform.Find("ServerName/NameValue").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI numOfPlayersText = newCell.transform.Find("NumberOfPlayers/PlayersValue").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI gameLenghtText = newCell.transform.Find("GameLenght/LenghtValue").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI mapSizeText = newCell.transform.Find("MapSize/SizeValue").GetComponent<TMPro.TextMeshProUGUI>();

            serverNameText.SetText(serverInfo.serverName);
            numOfPlayersText.SetText(serverInfo.numberOfPlayers.ToString());
            gameLenghtText.SetText(serverInfo.numberOfTurns.ToString() + " TURNS");
            mapSizeText.SetText(serverInfo.mapSize.ToString() + " x " + serverInfo.mapSize.ToString() + " Cells");
            newCell.GetComponent<ServerInfoReference>().setTCPInfo(serverInfo);
        }
    }

}
