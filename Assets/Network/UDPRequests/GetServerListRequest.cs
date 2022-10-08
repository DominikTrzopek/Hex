using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GetServerListRequest : IUDPREquest
{
    public int id;
    public string requestType = "get";

    public GetServerListRequest(int id){
        this.id = id;
    }

    public string getRequestType(){
        return requestType;
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
