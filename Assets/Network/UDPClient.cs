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

    public void Init()
    {
        remoteEndPoint = new IPEndPoint(
            IPAddress.Parse(UDPServerConfig.GetIp()),
            UDPServerConfig.GetPort()
        );
        client = new UdpClient();
        client.Client.SendBufferSize = 8192;
        client.Client.ReceiveBufferSize = 8192;
        client.Client.ReceiveTimeout = 4000;
        client.Client.SendTimeout = 2000;
    }

    public void SendData(IUDPREquest request)
    {
        try
        {
            Debug.Log(request.GetRequestType());
            string text = request.SaveToString();
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

    public byte[] ReceiveData()
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