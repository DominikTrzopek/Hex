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
    private TMPro.TMP_InputField customMapName;

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

    private string GetCustomMap(int size)
    {
        if (useCustomMap.isOn) 
        {
            Texture2D level;
            try
            {
                level = CustomMapLogic.Load(customMapName.text);
            }
            catch(Exception err)
            {
                Debug.Log(err);
                ErrorHandling.Handle(ResponseType.FILENOTFOUND, this.gameObject);
                return null;
            }
            level = CustomMapLogic.Scale(level, size, size);
            float[] levelVal = CustomMapLogic.GetGreyScale(level, size);
            System.Int16[] levelMap = CustomMapLogic.ConvertToTerrainLevelMap(levelVal, TerrainType.levels);
            String[] compressed = CustomMapLogic.CompressData(levelMap);
            string stringMap = CustomMapLogic.SerializeToString(compressed);
            if(stringMap.Length * sizeof(Char) > 4092)
            {
                Debug.Log(levelMap.Length);
                ErrorHandling.Handle(ResponseType.MAPSIZETOLARGE, this.gameObject);
                return null;
            }
            return stringMap;
        }
        return null;
    }

    private TCPServerInfo SetServerInfo(int seed, string customMap)
    {
        return new TCPServerInfo(
            UDPServerConfig.GetSecretId(),
            serverName.text,
            serverPassword.text,
            int.Parse(numberOfPlayers.options[numberOfPlayers.value].text),
            int.Parse(numberOfTurns.options[numberOfTurns.value].text),
            seed,
            mapType.options[mapType.value].text,
            int.Parse(mapSize.options[mapSize.value].text),
            customMap,
            startingNumberOfConnections
        );
    }


    public void RequestNewGameServer()
    {
        int seed = UnityEngine.Random.Range(-10000, 10000);
        string customMap = GetCustomMap(int.Parse(mapSize.options[mapSize.value].text));
        if (useCustomMap.isOn && customMap == null)
            return;
        serverInfo = SetServerInfo(seed, customMap);
        Debug.Log(serverInfo.SaveToString());
        new Thread(() =>
        {
            UDPClient client = new UDPClient();
            try
            {
                client.Init();
                client.SendData(new CreateServerRequest(serverInfo));
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
                byte[] responseByte = client.ReceiveData();
                string message = Encoding.Default.GetString(responseByte);
                UDPResponse response = UDPResponse.FromString(message);
                serverInfo = response.serverInfo;
                UDPServerConfig.SetSecretHash(serverInfo.creatorId);
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
                ErrorHandling.Handle(responseCode, this.gameObject);
                return;
            }
            TCPConnection conn = TCPConnection.instance;
            try
            {
                conn.ConnectToGame(serverInfo, serverPassword.text);
            }
            catch
            {
                conn.ClearConnection();
                return;
            }
            this.gameObject.SetActive(false);
            newView.SetActive(true);
        }
    }

}
