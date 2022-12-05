using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInfoReference : MonoBehaviour
{
    private TCPServerInfo tCPServerInfo;
    private string tCPPassword;

    public void setTCPPassword(string password){
        tCPPassword = password;
    }

    public string getTCPPassword(){
        return tCPPassword;
    } 

    public void setTCPInfo(TCPServerInfo info){
        tCPServerInfo = info;
    }

    public TCPServerInfo getTCPInfo(){
        return tCPServerInfo;
    }
}
