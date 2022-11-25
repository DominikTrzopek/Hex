using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class UDPClient
{
    IPEndPoint remoteEndPoint;
    UdpClient client;

    public void init()
    {
        remoteEndPoint = new IPEndPoint(
            IPAddress.Parse(UDPServerConfig.getIp()),
            UDPServerConfig.getPort()
        );
        //Debug.Log(remoteEndPoint.Port);
        client = new UdpClient();
        client.Client.ReceiveTimeout = 4000;
        client.Client.SendTimeout = 2000;
    }

    public void sendData(IUDPREquest request)
    {
        try
        {
            Debug.Log(request.getRequestType());
            string text = request.saveToString();
            if (text != "")
            {
                byte[] data = Encoding.UTF8.GetBytes(text);
                client.Send(data, data.Length, remoteEndPoint);
            }
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public byte[] receiveData()
    {
        try
        {
            return client.Receive(ref remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
        return null;
    }
}