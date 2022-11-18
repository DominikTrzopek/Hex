using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionScreenLogic : MonoBehaviour
{
    private bool ready = false;

    public void SetReadyStatus()
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
        Debug.Log(new ConnectMsg(info).saveToString());
        TCPConnection conn = TCPConnection.instance;
        conn.client.writeSocket(new ConnectMsg(info));
    }
}
