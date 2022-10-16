using UnityEngine;

[System.Serializable]
public class TCPServerInfo
{
    public int creatorId;
    public string password;
    public int numberOfPlayers;
    public int numberOfTurns;
    public int seed;
    public int mapType;
    public string ip;
    public int[] ports;

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
