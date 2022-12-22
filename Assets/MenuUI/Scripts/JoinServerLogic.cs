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
        TCPServerInfo info = GetTCPInfo();
        TCPConnection conn = TCPConnection.instance;
        try
        {
            conn.ConnectToGame(info, GetTCPPass());
        }
        catch
        {
            conn.ClearConnection();
            return;
        }
        SetScene();
    }

    private void SetScene()
    {
        GameObject oldScene = child.transform.parent.gameObject.GetComponent<ServerSceneReferences>().oldScene;
        GameObject newScene = child.transform.parent.gameObject.GetComponent<ServerSceneReferences>().newScene;

        oldScene.SetActive(false);
        newScene.SetActive(true);
    }

    private TCPServerInfo GetTCPInfo()
    {
        return child.GetComponent<ServerInfoReference>().GetTCPInfo();
    }

    private string GetTCPPass(){
        return serverPass.text;
    }
}
