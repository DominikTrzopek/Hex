using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class JoinServerLogic : MonoBehaviour
{

    [SerializeField]
    private GameObject child;

    public void JoinGame()
    {
        setScene();
        TCPServerInfo serverInfo = getTCPInfo();
        TCPClient client = new TCPClient();
        client.setupSocket(serverInfo.ip, serverInfo.ports[serverInfo.connections - 1]);
        client.writeSocket("this is message from unity :D");
        client.writeSocket("this is message from unity2 :D");
        client.writeSocket("this is message from unity3 :D");
        //TODO: wlasciwa wymiana danycg
        //client.closeSocket();
    }

    private void setScene()
    {
        GameObject oldScene = child.transform.parent.gameObject.GetComponent<ServerSceneReferences>().oldScene;
        GameObject newScene = child.transform.parent.gameObject.GetComponent<ServerSceneReferences>().newScene;

        newScene.SetActive(true);
        oldScene.SetActive(false);
    }

    private TCPServerInfo getTCPInfo()
    {
        return child.GetComponent<ServerInfoReference>().getTCPInfo();
    }
}
