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
        Debug.Log(TCPConnection.instance.selfNumber);
        if (ready == false)
        {
            ready = true;
            sendStatus(new PlayerInfo(PlayerStatus.READY, TCPConnection.instance.selfNumber));
            return;
        }
        else
        {
            ready = false;
            sendStatus(new PlayerInfo(PlayerStatus.NOTREADY, TCPConnection.instance.selfNumber));
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
            try
            {
                client.init();
                client.sendData(new DeleteServerRequest(TCPConnection.instance.serverInfo.pid));
                byte[] responseByte = client.receiveData();
                string message = Encoding.Default.GetString(responseByte);
                UDPResponse response = UDPResponse.fromString(message);
                Debug.Log(message);
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
            TCPConnection.instance.clearConnection();

        }).Start();

    }

    

}
