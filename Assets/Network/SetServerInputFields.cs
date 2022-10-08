using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetServerInputFields : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_InputField  portInputField;
    [SerializeField]
    private TMPro.TMP_InputField ipInputField;

    void Start()
    {
        portInputField.text = UDPServerConfig.getPort().ToString();
        ipInputField.text = UDPServerConfig.getIp();
    }

}
