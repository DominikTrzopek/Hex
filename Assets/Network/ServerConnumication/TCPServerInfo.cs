using UnityEngine;

[System.Serializable]
public class TCPServerInfo
{
    public int creatorId;
    public string serverName;
    public string password;
    public int numberOfPlayers;
    public int numberOfTurns;
    public int seed;
    public int mapType;
    public int mapSize;
    public int[] customMap;
    public string ip;
    public int[] ports;
    public int connections;

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public TCPServerInfo() { }

    public TCPServerInfo(int creatorId, string serverName,
    string password, int numberOfPlayers, int numberOfTurns,
    int seed, int mapType, int mapSize, int[] customMap, int connections)
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
