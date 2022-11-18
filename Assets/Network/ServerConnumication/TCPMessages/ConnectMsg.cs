using UnityEngine;

[System.Serializable]
public class ConnectMsg : ITCPMsg
{
    public PlayerInfo playerInfo;
    public string password;

    public ConnectMsg(){}

    public ConnectMsg(PlayerInfo info)
    {
        this.playerInfo = info;
    }

    public ConnectMsg(PlayerInfo info, string pass)
    {
        this.playerInfo = info;
        this.password = pass;
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public static ConnectMsg fromString(string json){
        return JsonUtility.FromJson<ConnectMsg>(json);
    }


}
