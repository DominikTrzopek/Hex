using UnityEngine;

[System.Serializable]
public class ConnectMsg : ITCPMsg
{
    public PlayerInfo playerInfo;
    public string password;
    public ResponseType code;

    public ConnectMsg() { }

    public ConnectMsg(PlayerInfo info)
    {
        this.playerInfo = info;
    }

    public ConnectMsg(PlayerInfo info, string pass)
    {
        this.playerInfo = info;
        this.password = pass;
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public static ConnectMsg FromString(string json)
    {
        return JsonUtility.FromJson<ConnectMsg>(json);
    }


}
