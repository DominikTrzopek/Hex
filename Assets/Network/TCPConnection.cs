using UnityEngine;
using System.Collections.Generic;

public class TCPConnection : MonoBehaviour
{
    public static TCPConnection instance { get; private set; }
    public TCPClient client;
    public TCPServerInfo serverInfo;
    public List<PlayerInfo> playerInfo = new List<PlayerInfo>();

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
}