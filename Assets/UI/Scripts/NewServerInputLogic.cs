using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Text;
using System;


public class NewServerInputLogic : MonoBehaviour
{

    const int startingNumberOfConnections = 1;

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

    private int[] GetCustomMap()
    {
        if (!useCustomMap.isOn) //inversed value
        {
            //todo load, calculate and compress bitmap
            return new int[10];
        }
        return null;
    }

    public TCPServerInfo setServerInfo(int seed)
    {
        return new TCPServerInfo(
            UDPServerConfig.getId(),
            serverName.text,
            serverPassword.text,
            int.Parse(numberOfPlayers.options[numberOfPlayers.value].text),
            int.Parse(numberOfTurns.options[numberOfTurns.value].text),
            seed,
            int.Parse(mapType.options[mapType.value].text),
            int.Parse(mapSize.options[mapSize.value].text),
            GetCustomMap(),
            startingNumberOfConnections
        );
    }

    private static Mutex mutex = new Mutex();

    public void RequestNewGameServer()
    {
        int seed = UnityEngine.Random.Range(-10000, 10000);
        TCPServerInfo serverInfo = setServerInfo(seed);
        Thread thread = new Thread(() =>
        {
            mutex.WaitOne();
            UDPClient client = new UDPClient();
            client.init();
            client.sendData(new CreateServerRequest(serverInfo));
            try
            {
                byte[] responseByte = client.receiveData();
                string message = Encoding.Default.GetString(responseByte);
                UDPResponse response = UDPResponse.fromString(message);
                Debug.Log(message);
                serverInfo = response.serverInfo;
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
            mutex.ReleaseMutex();
        });

        thread.Start();
        thread.Join();
        TCPConnection conn = TCPConnection.instance;
        conn.connectToGame(serverInfo, serverInfo.password);



    }

}
