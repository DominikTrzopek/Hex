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

    public void clear()
    {
        instance = null;
    }

    public void connectToGame(TCPServerInfo info, string password)
    {
        serverInfo = info;
        foreach (int port in info.ports)
        {
            try
            {
                client.setupSocket(info.ip, port);
                client.readSocket();
                if (!client.socketReady)
                {
                    client.closeSocket();
                    throw new System.IO.IOException();
                }
                client.setTimeout(300);
                client.writeSocket(buildConnectMsg(password));
                receiverThread = new Thread(new ThreadStart(receiveData));
                receiverThread.Start();
                return;
                
            }
            catch (System.IO.IOException)
            {
                Debug.Log("Port in use");
            }
        }
    }

    private ConnectMsg buildConnectMsg(string password)
    {
        PlayerInfo info = new PlayerInfo(
            UDPServerConfig.getId(),
            UDPServerConfig.getSecretId(),
            UDPServerConfig.getPlayerName(),
            PlayerStatus.NOTREADY,
            Color.black
        );
        return new ConnectMsg(info, password);
    }

    private void receiveData()
    {
        new Thread(() =>
        {
            while (client.socketReady)
            {
                byte[] bytes = client.readSocket();
                if (bytes != null)
                {
                    string message = Encoding.Default.GetString(bytes);
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
                }
                else
                {
                    messageQueue.Add("Zerwano polaczenie");
                }
            }
        }).Start();
    }

    public void clearConnection()
    {
        client.closeSocket();
        playerInfo = new List<PlayerInfo>();
        messageQueue = new List<string>();
        serverInfo = null;
        client = new TCPClient();
        if(receiverThread != null)
            receiverThread.Join();
    }
}