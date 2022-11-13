using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITCPMsg
{
    public string saveToString();
    public static ConnectMsg fromString(string json) => new ConnectMsg();
    
}
