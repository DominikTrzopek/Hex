using UnityEngine;

[System.Serializable]
public class GetServerListRequest : IUDPREquest
{
    public string id;
    public RequestType requestType = RequestType.GET;

    public GetServerListRequest(){
        this.id = UDPServerConfig.GetId();
    }

    public string GetRequestType(){
        return requestType.ToString();
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
