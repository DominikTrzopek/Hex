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
        TCPServerInfo serverInfo = getTCPInfo();
        TCPConnection.serverInfo = serverInfo;
        setScene();
        TCPConnection.client = new TCPClient();
        TCPConnection.client.setupSocket(serverInfo.ip, serverInfo.ports[serverInfo.connections - 1]);
        TCPConnection.client.writeSocket("this is message from unity :D");

        //TODO: wlasciwa wymiana danycg
        //client.closeSocket();
    }

    private void setScene()
    {
        GameObject oldScene = child.transform.parent.gameObject.GetComponent<ServerSceneReferences>().oldScene;
        GameObject newScene = child.transform.parent.gameObject.GetComponent<ServerSceneReferences>().newScene;

        oldScene.SetActive(false);
        newScene.SetActive(true);
    }

    private TCPServerInfo getTCPInfo()
    {
        return child.GetComponent<ServerInfoReference>().getTCPInfo();
    }
}
