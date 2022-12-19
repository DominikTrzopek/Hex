using System.Text;
using System.Threading;
using UnityEngine;
using System;
using System.Collections.Generic;


public class ConnectionScreenLogic : MonoBehaviour
{
    private bool ready = false;

    public void SetReadyStatus()
    {
        if (ready == false)
        {
            ready = true;
            SendStatus(new PlayerInfo(PlayerStatus.READY, TCPConnection.instance.selfNumber));
            return;
        }
        else
        {
            ready = false;
            SendStatus(new PlayerInfo(PlayerStatus.NOTREADY, TCPConnection.instance.selfNumber));
        }

    }

    private void SendStatus(PlayerInfo info)
    {
        TCPConnection conn = TCPConnection.instance;
        conn.client.WriteSocket(new ConnectMsg(info));
    }

    public void SendDeleteRequest()
    {
        new Thread(() =>
        {
            Debug.Log(TCPConnection.instance.serverInfo.pid);
            UDPClient client = new UDPClient();
            try
            {
                client.Init();
                client.SendData(new DeleteServerRequest(TCPConnection.instance.serverInfo.pid));
                byte[] responseByte = client.ReceiveData();
                string message = Encoding.Default.GetString(responseByte);
                UDPResponse response = UDPResponse.FromString(message);
                Debug.Log(message);
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
            TCPConnection.instance.ClearConnection();

        }).Start();

    }

    

}
