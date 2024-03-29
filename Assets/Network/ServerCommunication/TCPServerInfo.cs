using UnityEngine;

[System.Serializable]
public class TCPServerInfo
{
    public string creatorId;
    public string serverName;
    public string password;
    public int numberOfPlayers;
    public int numberOfTurns;
    public int seed;
    public string mapType;
    public int mapSize;
    public string customMap;
    public string ip;
    public int[] ports;
    public int connections;
    public int pid;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public TCPServerInfo() { }

    public TCPServerInfo(string creatorId, string serverName,
    string password, int numberOfPlayers, int numberOfTurns,
    int seed, string mapType, int mapSize, string customMap, int connections)
    {
        this.creatorId = creatorId;
        this.serverName = serverName;
        this.password = password;
        this.numberOfPlayers = numberOfPlayers;
        this.numberOfTurns = numberOfTurns;
        this.seed = seed;
        this.mapSize = mapSize;
        this.mapType = mapType;
        this.customMap = customMap;
        this.connections = connections;
    }
}
