using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoGetter : MonoBehaviour
{
    public static Color GetColor(string playerId)
    {
        TCPConnection conn = TCPConnection.instance;
        foreach(PlayerInfo info in conn.playerInfo)
        {
            if(playerId == info.id)
                return info.color;
        }
        return Color.grey;
    }

    public static string GetName(string playerId)
    {
        TCPConnection conn = TCPConnection.instance;
        foreach(PlayerInfo info in conn.playerInfo)
        {
            if(playerId == info.id)
                return info.name;
        }
        return "";
    }
}
