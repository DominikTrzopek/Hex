using UnityEngine;

[System.Serializable]
public class CreateServerRequest : IUDPREquest
{
    public RequestType requestType = RequestType.CREATE;
    public TCPServerInfo serverInfo = new TCPServerInfo();

    public CreateServerRequest(int id, string password, int numberOfPlayers, int numberOfTurns, int mapType, int seed){
        this.serverInfo.creatorId = id;
        this.serverInfo.password = password;
        this.serverInfo.numberOfPlayers = numberOfPlayers;
        this.serverInfo.numberOfTurns = numberOfPlayers;
        this.serverInfo.mapType = mapType;
        this.serverInfo.seed = seed;
    }

    public string getRequestType(){
        return requestType.ToString();
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
