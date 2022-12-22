using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInfoReference : MonoBehaviour
{
    private TCPServerInfo tCPServerInfo;
    private string tCPPassword;

    public void SetTCPPassword(string password){
        tCPPassword = password;
    }

    public string GetTCPPassword(){
        return tCPPassword;
    } 

    public void SetTCPInfo(TCPServerInfo info){
        tCPServerInfo = info;
    }

    public TCPServerInfo GetTCPInfo(){
        return tCPServerInfo;
    }
}
