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

    public string getRequestType()
    {
        return requestType.ToString();
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
