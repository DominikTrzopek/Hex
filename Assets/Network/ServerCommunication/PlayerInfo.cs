using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public string id;
    public Color color;
    public string name;
    public PlayerStatus status;
    public int number;
    public string secretId;

    public PlayerInfo(string id, string secretId, string name, PlayerStatus status, Color color){
        this.id = id;
        this.secretId = secretId;
        this.name = name;
        this.status = status;
        this.color = color;
    }

    public PlayerInfo(string id, string secretId){
        this.id = id;
        this.secretId = secretId;
    }

    public PlayerInfo(PlayerStatus status, int number){
        this.id = UDPServerConfig.GetId();
        this.name = UDPServerConfig.GetPlayerName();
        this.status = status;
        this.number = number;
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
