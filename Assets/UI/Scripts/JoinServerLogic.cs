using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Text;

public class JoinServerLogic : MonoBehaviour
{

    [SerializeField]
    private GameObject child;
    
    [SerializeField]
    private TMPro.TMP_InputField serverPass;

    public void JoinGame()
    {
        TCPServerInfo info = getTCPInfo();
        TCPConnection conn = TCPConnection.instance;
        conn.connectToGame(info, getTCPPass());
        setScene();
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

    private string getTCPPass(){
        return serverPass.text;
    }
}
