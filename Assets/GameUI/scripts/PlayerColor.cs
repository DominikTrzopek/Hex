using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
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
}
