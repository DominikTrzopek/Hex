using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteServerRequest : IUDPREquest
{
    public string playerid;
    public RequestType requestType = RequestType.DELETE;
    public int serverId;

    public DeleteServerRequest(int serverId){
        this.playerid = UDPServerConfig.getSecretId();
        this.serverId = serverId;
    }

    public string getRequestType(){
        return requestType.ToString();
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
