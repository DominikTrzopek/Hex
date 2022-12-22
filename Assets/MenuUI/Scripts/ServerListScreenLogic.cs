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
    private ResponseType responseCode;
    private bool dataReady = false;
    private bool refresh = false;
    List<TCPServerInfo> tcpServers;

    void OnEnable()
    {

        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        tcpServers = new List<TCPServerInfo>();

        new Thread(() =>
        {
            UDPClient client = new UDPClient();
            try
            {
                client.Init();
                client.SendData(new GetServerListRequest());
            }
            catch (Exception err)
            {
                responseCode = ResponseType.BADADDRESS;
                Debug.Log(err);
                dataReady = true;
                return;
            }
            while (true)
            {
                try
                {
                    byte[] byteResponse = client.ReceiveData();
                    string message = Encoding.Default.GetString(byteResponse);
                    UDPResponse udpResponse = UDPResponse.FromString(message);
                    if (udpResponse.responseType == ResponseType.ENDOFMESSAGE)
                    {
                        responseCode = ResponseType.ENDOFMESSAGE;
                        dataReady = true;
                        return;
                    }
                    responseCode = udpResponse.responseType;
                    tcpServers.Add(udpResponse.serverInfo);

                }
                catch (Exception err)
                {
                    Debug.Log(err.ToString());
                    responseCode = ResponseType.UDPSERVERDOWN;
                    dataReady = true;
                    return;
                }
            }
        }).Start();

    }

    void OnDisable()
    {
        dataReady = false;
    }

    public void Update()
    {
        if (dataReady)
        {
            dataReady = false;
            if (responseCode != ResponseType.SUCCESS && responseCode != ResponseType.ENDOFMESSAGE)
            {
                ErrorHandling.Handle(responseCode, this.transform.parent.parent.gameObject);
                return;
            }
            DisplayServerInfo(tcpServers);
        }
        if(refresh)
        {
            refresh = false;
            OnEnable();
        }
    }

    public void Refresh()
    {
        refresh = true;
    }

    private void DisplayServerInfo(List<TCPServerInfo> tcpServers)
    {

        foreach (var serverInfo in tcpServers)
        {
            GameObject newCell = Instantiate(CellPrefab);
            newCell.transform.SetParent(this.gameObject.transform, false);

            TMPro.TextMeshProUGUI serverNameText = newCell.transform.Find("ServerName/NameValue").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI numOfPlayersText = newCell.transform.Find("NumberOfPlayers/PlayersValue").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI gameLenghtText = newCell.transform.Find("GameLenght/LenghtValue").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI mapSizeText = newCell.transform.Find("MapSize/SizeValue").GetComponent<TMPro.TextMeshProUGUI>();

            serverNameText.SetText(serverInfo.serverName);
            numOfPlayersText.SetText(serverInfo.connections.ToString() + "/" + serverInfo.numberOfPlayers.ToString());
            gameLenghtText.SetText(serverInfo.numberOfTurns.ToString() + " TURNS");
            mapSizeText.SetText(serverInfo.mapSize.ToString() + " x " + serverInfo.mapSize.ToString() + " Cells");
            newCell.GetComponent<ServerInfoReference>().SetTCPInfo(serverInfo);
            if (serverInfo.password == null || serverInfo.password.Trim() == "")
            {
                newCell.transform.Find("Button/PasswordInputField").gameObject.SetActive(false);
            }
        }
    }

}
