using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandBuilder : ITCPMsg
{
    public string ownerId;
    public string networkId;
    public CommandEnum command;
    public List<string> args;
    public GameState gameState;

    public CommandBuilder(string networkId, CommandEnum command, List<string> args, GameState gameState)
    {
        this.ownerId = UDPServerConfig.GetId();
        this.networkId = networkId;
        this.command = command;
        this.args = args;
        this.gameState = gameState;
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public static CommandBuilder FromString(string json)
    {
        return JsonUtility.FromJson<CommandBuilder>(json);
    }
}
