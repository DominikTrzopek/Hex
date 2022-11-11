using UnityEngine;

[System.Serializable]
public class ConnectMsg : ITCPMsg
{
    public PlayerInfo playerInfo;
    public string password;

    public ConnectMsg(PlayerInfo info, string pass)
    {
        this.playerInfo = info;
        this.password = pass;
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
