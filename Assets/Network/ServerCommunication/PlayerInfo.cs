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

    public PlayerInfo(string id, string name, PlayerStatus status, Color color){
        this.id = id;
        this.name = name;
        this.status = status;
        this.color = color;
    }

    public PlayerInfo(PlayerStatus status){
        this.id = UDPServerConfig.getId();
        this.name = UDPServerConfig.getPlayerName();
        this.status = status;
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

}