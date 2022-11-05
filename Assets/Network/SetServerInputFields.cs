using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetServerInputFields : MonoBehaviour
{
    private static TMPro.TMP_InputField  portInputField;
    private static TMPro.TMP_InputField ipInputField;
    private static TMPro.TMP_InputField playerName;

    void Awake()
    {
        setInputFields();
        portInputField.text = UDPServerConfig.getPort().ToString();
        ipInputField.text = UDPServerConfig.getIp();
        playerName.text = UDPServerConfig.getPlayerName() + Random.Range(100, 1000).ToString();
    }

    public static void setInputFields()
    {
        portInputField = GameObject.Find("PortInputField").GetComponent<TMPro.TMP_InputField>();
        ipInputField = GameObject.Find("IpInputField").GetComponent<TMPro.TMP_InputField>();
        playerName = GameObject.Find("PlayerNameInputField").GetComponent<TMPro.TMP_InputField>();
    }

    public static void UpdateData()
    {
        UDPServerConfig.setPort(int.Parse(portInputField.text));
        UDPServerConfig.setIP(ipInputField.text);
        UDPServerConfig.setPlayerName(playerName.text);
    }

}