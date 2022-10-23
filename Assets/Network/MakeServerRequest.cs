using UnityEngine;
using System.Text;
using System.Threading;
using System;

public class MakeServerRequest : MonoBehaviour
{

    private static Mutex mutex = new Mutex();

    public void RequestNewGameServer()
    {
        // new Thread(() =>
        // {
        //     mutex.WaitOne();
        //     UDPClient client = new UDPClient();
        //     client.init();
        //     client.sendData(new CreateServerRequest(1, "test", "pass", 3, 4, 5, 1));
        //     try
        //     {
        //         byte[] response = client.receiveData();
        //         string message = Encoding.Default.GetString(response);
        //         UDPResponse response1 = UDPResponse.fromString(message);
        //     }
        //     catch (Exception err)
        //     {
        //         Debug.Log(err.ToString());
        //     }
        //     mutex.ReleaseMutex();
        // }).Start();
    }

    public void RequestServerList()
    {

        new Thread(() =>
        {
            mutex.WaitOne();
            UDPClient client = new UDPClient();
            client.init();
            client.sendData(new GetServerListRequest(1));
            while (true)
            {
                try
                {
                    byte[] response = client.receiveData();
                    string message = Encoding.Default.GetString(response);
                    UDPResponse response2 = UDPResponse.fromString(message);
                    if (response2.responseType == ResponseType.ENDOFMESSAGE)
                    {
                        mutex.ReleaseMutex();
                        return;
                    }
                }
                catch (Exception err)
                {
                    Debug.Log(err.ToString());
                    mutex.ReleaseMutex();
                    return;
                }
            }
        }).Start();
    }





}
