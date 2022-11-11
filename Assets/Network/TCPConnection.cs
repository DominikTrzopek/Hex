using UnityEngine;

public class TCPConnection : MonoBehaviour
{
    public static TCPConnection instance { get; private set; }
    public TCPClient client;
    public TCPServerInfo serverInfo;

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

    public void connectToGame(TCPServerInfo info, ConnectMsg connectMsg)
    {
        serverInfo = info;
        client.setupSocket(info.ip, info.ports[info.connections - 1]);
        client.writeSocket(connectMsg);
    }
}