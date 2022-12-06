using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandBuilder
{
    public string ownerId;
    public string networkId;
    public CommandEnum command;
    public List<string> args;

    public CommandBuilder(string networkId, CommandEnum command, List<string> args)
    {
        this.ownerId = UDPServerConfig.getId();
        this.networkId = networkId;
        this.command = command;
        this.args = args;
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public static ConnectMsg fromString(string json)
    {
        return JsonUtility.FromJson<ConnectMsg>(json);
    }
}
