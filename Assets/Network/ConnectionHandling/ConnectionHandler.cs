using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour
{
    public GameObject errorPannel;
    void Update()
    {
        StartCoroutine(Coroutine());
        if(TCPConnection.instance.client.socketReady)
        {
            Debug.Log("Connected");
            errorPannel.SetActive(false);
        }
        else
        {
            errorPannel.SetActive(true);
        }

        if (Input.GetKey("q"))
        {
            TCPConnection.instance.client.disconnect();
            TCPConnection.instance.client.closeSocket();
        }
    }

    public void Reconnect()
    {
        TCPConnection.instance.Reconnect();
    }

    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(1);
    }
}
