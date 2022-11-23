using System.Text;
using System.Threading;
using UnityEngine;
using System;
using System.Collections.Generic;


public class ConnectionScreenLogic : MonoBehaviour
{
    private bool ready = false;

    public void setReadyStatus()
    {
        if (ready == false)
        {
            ready = true;
            sendStatus(new PlayerInfo(PlayerStatus.READY));
            return;
        }
        else
        {
            ready = false;
            sendStatus(new PlayerInfo(PlayerStatus.NOTREADY));
        }

    }

    private void sendStatus(PlayerInfo info)
    {
        TCPConnection conn = TCPConnection.instance;
        conn.client.writeSocket(new ConnectMsg(info));
    }

    public void sendDeleteRequest()
    {
        new Thread(() =>
        {
            Debug.Log(TCPConnection.instance.serverInfo.pid);
            UDPClient client = new UDPClient();
            client.init();
            client.sendData(new DeleteServerRequest(TCPConnection.instance.serverInfo.pid));
            try
            {
                byte[] responseByte = client.receiveData();
                string message = Encoding.Default.GetString(responseByte);
                UDPResponse response = UDPResponse.fromString(message);
                Debug.Log(message);
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }

            clearConnection();

        }).Start();

    }

    public void clearConnection()
    {
        TCPConnection.instance.client.closeSocket();
        TCPConnection.instance.playerInfo = new List<PlayerInfo>();
        TCPConnection.instance.messageQueue = new List<string>();
        TCPConnection.instance.serverInfo = null;
        TCPConnection.instance.client = new TCPClient();
        TCPConnection.instance.receiverThread.Join();
    }
}
