using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInfoReference : MonoBehaviour
{
    private TCPServerInfo tCPServerInfo;

    public void setTCPInfo(TCPServerInfo info){
        tCPServerInfo = info;
    }

    public TCPServerInfo getTCPInfo(){
        return tCPServerInfo;
    }
}
