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
    public Thread receiverThread;

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
        client.setupSocket(info.ip, info.ports[0]);
        client.writeSocket(buildConnectMsg(password));
        receiverThread = new Thread(new ThreadStart(receiveData));
        receiverThread.Start();
    }

    private ConnectMsg buildConnectMsg(string password)
    {
        PlayerInfo info = new PlayerInfo(
            UDPServerConfig.getId(),
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
                    string[] splieted = message.Split("\n");
                    foreach (string part in splieted)
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
            }
        }).Start();
    }
}