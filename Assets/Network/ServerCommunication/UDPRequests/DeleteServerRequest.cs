using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteServerRequest : IUDPREquest
{
    public string playerid;
    public RequestType requestType = RequestType.DELETE;
    public int serverId;

    public DeleteServerRequest(int serverId){
        this.playerid = UDPServerConfig.GetSecretId();
        this.serverId = serverId;
    }

    public string GetRequestType(){
        return requestType.ToString();
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
