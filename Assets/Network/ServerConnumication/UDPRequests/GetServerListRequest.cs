using UnityEngine;

[System.Serializable]
public class GetServerListRequest : IUDPREquest
{
    public string id;
    public RequestType requestType = RequestType.GET;

    public GetServerListRequest(){
        this.id = UDPServerConfig.getId();
    }

    public string getRequestType(){
        return requestType.ToString();
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
