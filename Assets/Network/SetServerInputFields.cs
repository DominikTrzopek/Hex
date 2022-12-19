using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SetServerInputFields : MonoBehaviour
{
    private static TMPro.TMP_InputField  portInputField;
    private static TMPro.TMP_InputField ipInputField;
    private static TMPro.TMP_InputField playerName;

    void Awake()
    {
        SetInputFields();
        portInputField.text = UDPServerConfig.GetPort().ToString();
        ipInputField.text = UDPServerConfig.GetIp();
        if(UDPServerConfig.GetPlayerName().Trim() != "" && UDPServerConfig.GetPlayerName().Trim() != "player")
            playerName.text = UDPServerConfig.GetPlayerName();
        else
            playerName.text = "player" + UnityEngine.Random.Range(100, 1000).ToString();
    }

    public static void SetInputFields()
    {
        portInputField = GameObject.Find("PortInputField").GetComponent<TMPro.TMP_InputField>();
        ipInputField = GameObject.Find("IpInputField").GetComponent<TMPro.TMP_InputField>();
        playerName = GameObject.Find("PlayerNameInputField").GetComponent<TMPro.TMP_InputField>();
    }

    public static void UpdateData()
    {
        UDPServerConfig.SetPort(int.Parse(portInputField.text));
        UDPServerConfig.SetIP(ipInputField.text);
        UDPServerConfig.SetPlayerName(playerName.text);
    }

    void OnEnable()
    {
        try
        {
            TCPConnection conn = TCPConnection.instance;
            conn.ClearConnection();
        }
        catch(NullReferenceException){}
    }

}
