using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandMsg : ITCPMsg
{
    public string ownerId;
    public string secretId;
    public string objectId;
    public CommandEnum command;
    public List<string> args;

    public CommandMsg(){}

    public CommandMsg(string objectId, CommandEnum command, List<string> args)
    {
        this.ownerId = UDPServerConfig.getId();
        this.secretId = UDPServerConfig.getSecretId();
        this.objectId = objectId;
        this.command = command;
        this.args = args;
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public static CommandMsg fromString(string json){
        return JsonUtility.FromJson<CommandMsg>(json);
    }
}