using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinServerLogic : MonoBehaviour
{

    [SerializeField]
    private GameObject child;

    public void JoinGame(){

        GameObject oldScene = child.transform.parent.gameObject.GetComponent<ServerSceneReferences>().oldScene;
        GameObject newScene = child.transform.parent.gameObject.GetComponent<ServerSceneReferences>().newScene;

        newScene.SetActive(true);
        oldScene.SetActive(false);

        TCPServerInfo serverInfo = child.GetComponent<ServerInfoReference>().getTCPInfo();
        Debug.Log(serverInfo.serverName);
        //TODO: connect to tcp server
    }
}
