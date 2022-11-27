using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Text;
using System;
using System.Net.Sockets;

public class NewServerInputLogic : MonoBehaviour
{

    const int startingNumberOfConnections = 0;

    [SerializeField]
    private TMPro.TMP_InputField serverName;
    [SerializeField]
    private TMPro.TMP_InputField serverPassword;
    [SerializeField]
    private TMPro.TMP_Dropdown numberOfPlayers;
    [SerializeField]
    private TMPro.TMP_Dropdown numberOfTurns;
    [SerializeField]
    private TMPro.TMP_Dropdown mapSize;
    [SerializeField]
    private TMPro.TMP_Dropdown mapType;
    [SerializeField]
    private Toggle useCustomMap;
    [SerializeField]
    private GameObject newView;
    private bool dataReady = false;
    private TCPServerInfo serverInfo;
    private ResponseType responseCode;

    private int[] GetCustomMap()
    {
        if (!useCustomMap.isOn) //inversed value
        {
            //todo load, calculate and compress bitmap
            return new int[10];
        }
        return null;
    }

    private TCPServerInfo setServerInfo(int seed)
    {
        return new TCPServerInfo(
            UDPServerConfig.getSecretId(),
            serverName.text,
            serverPassword.text,
            int.Parse(numberOfPlayers.options[numberOfPlayers.value].text),
            int.Parse(numberOfTurns.options[numberOfTurns.value].text),
            seed,
            mapType.options[mapType.value].text,
            int.Parse(mapSize.options[mapSize.value].text),
            GetCustomMap(),
            startingNumberOfConnections
        );
    }


    public void RequestNewGameServer()
    {
        int seed = UnityEngine.Random.Range(-10000, 10000);
        serverInfo = setServerInfo(seed);
        new Thread(() =>
        {
            UDPClient client = new UDPClient();
            try
            {
                client.init();
                client.sendData(new CreateServerRequest(serverInfo));
            }
            catch(Exception err)
            {
                responseCode = ResponseType.BADADDRESS;
                Debug.Log(err);
                dataReady = true;
                return;
            }
            try
            {
                byte[] responseByte = client.receiveData();
                string message = Encoding.Default.GetString(responseByte);
                UDPResponse response = UDPResponse.fromString(message);
                serverInfo = response.serverInfo;
                UDPServerConfig.setSecretHash(serverInfo.creatorId);
                responseCode = response.responseType;
                dataReady = true;
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
                dataReady = true;
                responseCode = ResponseType.UDPSERVERDOWN;
            }
        }).Start();
    }

    void OnDisable()
    {
        dataReady = false;
    }


    public void Update()
    {
        if(dataReady)
        {
            dataReady = false;
            if(responseCode != ResponseType.SUCCESS)
            {
                Debug.Log(this.gameObject.name);
                ErrorHandling.handle(responseCode, this.gameObject);
                return;
            }
            TCPConnection conn = TCPConnection.instance;
            try
            {
                conn.connectToGame(serverInfo, serverPassword.text);
            }
            catch
            {
                conn.clearConnection();
                return;
            }
            this.gameObject.SetActive(false);
            newView.SetActive(true);
        }
    }

}
