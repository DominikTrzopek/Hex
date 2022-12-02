using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandling : MonoBehaviour
{
    public static void handle(ResponseType code, GameObject currentView)
    {
        switch (code)
        {
            case ResponseType.WRONGPASSWORD:
                setView(currentView, "Authorization failed, wrong password!");
                break;
            case ResponseType.BADCONNECTION:
                setView(currentView, "Authorization failed, bad connect message!");
                break;
            case ResponseType.BADPLAYERDATA:
                setView(currentView, "Authorization failed, missing player data!");
                break;
            case ResponseType.BADREQUEST:
                setView(currentView, "Bad request, missing request type!");
                break;
            case ResponseType.BADARGUMENTS:
                setView(currentView, "Bad request, missing request data!");
                break;
            case ResponseType.TCPSERVERFAIL:
                setView(currentView, "TCP server failed!");
                break;
            case ResponseType.DISCONNECT:
                setView(currentView, "Disconnected from server!");
                break;
            case ResponseType.UDPSERVERDOWN:
                setView(currentView, "UDP server is down!");
                break;
            case ResponseType.BADADDRESS:
                setView(currentView, "Bad UDP socket address!");
                break;
            case ResponseType.FILENOTFOUND:
                setView(currentView, "Map file not found! Put image inside game folder");
                break;
            case ResponseType.MAPSIZETOLARGE:
                setView(currentView, "Map array is to large! Set smaller map size");
                break;
        }
    }

    private static void setView(GameObject currentView, string errorText)
    {
        GameObject errorScreen = GameObject.Find("Canvas").transform.Find("ErrorScreen").gameObject;
        errorScreen.SetActive(true);
        errorScreen.transform.Find("ErrorMessage").GetComponent<TMPro.TextMeshProUGUI>().text = errorText;
        currentView.transform.gameObject.SetActive(false);
    }
}
