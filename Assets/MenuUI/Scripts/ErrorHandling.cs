using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandling : MonoBehaviour
{
    public static void Handle(ResponseType code, GameObject currentView)
    {
        switch (code)
        {
            case ResponseType.WRONGPASSWORD:
                SetView(currentView, "Authorization failed, wrong password!");
                break;
            case ResponseType.BADCONNECTION:
                SetView(currentView, "Authorization failed, bad connect message!");
                break;
            case ResponseType.BADPLAYERDATA:
                SetView(currentView, "Authorization failed, missing player data!");
                break;
            case ResponseType.BADREQUEST:
                SetView(currentView, "Bad request, missing request type!");
                break;
            case ResponseType.BADARGUMENTS:
                SetView(currentView, "Bad request, missing request data!");
                break;
            case ResponseType.TCPSERVERFAIL:
                SetView(currentView, "TCP server failed!");
                break;
            case ResponseType.DISCONNECT:
                SetView(currentView, "Disconnected from server!");
                break;
            case ResponseType.UDPSERVERDOWN:
                SetView(currentView, "UDP server is down!");
                break;
            case ResponseType.BADADDRESS:
                SetView(currentView, "Bad UDP socket address!");
                break;
            case ResponseType.FILENOTFOUND:
                SetView(currentView, "Map file not found! Put image inside game folder");
                break;
            case ResponseType.MAPSIZETOLARGE:
                SetView(currentView, "Map array is to large! Set smaller map size");
                break;
        }
    }

    private static void SetView(GameObject currentView, string errorText)
    {
        GameObject errorScreen = GameObject.Find("Canvas").transform.Find("ErrorScreen").gameObject;
        errorScreen.SetActive(true);
        errorScreen.transform.Find("ErrorMessage").GetComponent<TMPro.TextMeshProUGUI>().text = errorText;
        currentView.transform.gameObject.SetActive(false);
    }
}
