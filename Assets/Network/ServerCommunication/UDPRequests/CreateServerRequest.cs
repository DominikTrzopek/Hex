using UnityEngine;

[System.Serializable]
public class CreateServerRequest : IUDPREquest
{
    public RequestType requestType = RequestType.CREATE;
    public TCPServerInfo serverInfo = new TCPServerInfo();

    public CreateServerRequest(TCPServerInfo serverInfo)
    {
        this.serverInfo = serverInfo;
    }

    public string GetRequestType()
    {
        return requestType.ToString();
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
