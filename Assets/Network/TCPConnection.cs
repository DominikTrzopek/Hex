using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System;

public class TCPConnection : MonoBehaviour
{
    public static TCPConnection instance { get; private set; }
    public TCPClient client;
    public TCPServerInfo serverInfo;
    public List<PlayerInfo> playerInfo = new List<PlayerInfo>();
    public List<string> messageQueue = new List<string>();
    private Thread receiverThread;
    public int selfNumber;
    public int selfPort;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    public void Clear()
    {
        instance = null;
    }

    public void ConnectToGame(TCPServerInfo info, string password)
    {
        serverInfo = info;
        foreach (int port in info.ports)
        {
            try
            {
                client.SetupSocket(info.ip, port);
                client.ReadSocket();
                if (!client.socketReady)
                {
                    client.CloseSocket();
                    throw new System.IO.IOException();
                }
                client.SetTimeout(300);
                client.WriteSocket(BuildConnectMsg(password));
                receiverThread = new Thread(new ThreadStart(ReceiveData));
                receiverThread.Start();
                selfPort = port;
                return;

            }
            catch (System.IO.IOException)
            {
                Debug.Log("Port in use");
            }
        }
    }

    private ConnectMsg BuildConnectMsg(string password)
    {
        PlayerInfo info = new PlayerInfo(
            UDPServerConfig.GetId(),
            UDPServerConfig.GetSecretId(),
            UDPServerConfig.GetPlayerName(),
            PlayerStatus.NOTREADY,
            Color.black
        );
        return new ConnectMsg(info, password);
    }

    public static ConnectMsg BuildReconnectMsg()
    {
        PlayerInfo info = new PlayerInfo(
            UDPServerConfig.GetId(),
            UDPServerConfig.GetSecretId()
        );
        return new ConnectMsg(info);
    }

    public void Reconnect()
    {
        TCPConnection conn = TCPConnection.instance;
        conn.client.SetupSocket(conn.serverInfo.ip, conn.selfPort);
        conn.client.WriteSocket(TCPConnection.BuildReconnectMsg());
        receiverThread = new Thread(new ThreadStart(ReceiveData));
        receiverThread.Start();
    }


    private void ReceiveData()
    {
        new Thread(() =>
        {
            string message = "";
            while (client.socketReady)
            {
                byte[] bytes = client.ReadSocket();
                if (bytes != null)
                {
                    message += Encoding.Default.GetString(bytes);
                    if (message.Contains("\n"))
                    {
                        string[] splited = message.Split("\n");
                        foreach (string part in splited)
                        {
                            try
                            {
                                messageQueue.Add(part);
                            }
                            catch (ArgumentException err)
                            {
                                Debug.Log(err.ToString());
                            }
                        }
                        message = "";
                    }
                }
                else
                {
                    // client.socketReady = false;
                    // messageQueue.Add("Zerwano polaczenie");
                }
            }
        }).Start();
    }

    public void ClearConnection()
    {
        client.Disconnect();
        client.CloseSocket();
        playerInfo = new List<PlayerInfo>();
        messageQueue = new List<string>();
        serverInfo = null;
        client = new TCPClient();
        if (receiverThread != null)
            receiverThread.Join();
    }
}