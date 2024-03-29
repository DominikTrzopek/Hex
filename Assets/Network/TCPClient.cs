using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

[System.Serializable]
public class TCPClient
{
    internal Boolean socketReady = false;
    TcpClient client;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;

    public TCPClient() {}

    public void SetupSocket(String host, Int32 port)
    {
        try
        {
            client = new TcpClient(host, port);
            client.SendBufferSize = 8192;
            client.ReceiveBufferSize = 8192;
            client.SendTimeout = 2000;
            client.ReceiveTimeout = 2000;
            theStream = client.GetStream();
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
        }
    }

    public void SetTimeout(int seconds)
    {
        client.SendTimeout = seconds * 1000;
        client.ReceiveTimeout = seconds * 1000;
    }

    public void WriteSocket(ITCPMsg msg)
    {
        try
        {
            if (!socketReady)
                return;
            theWriter.Write(msg.SaveToString() + "\n");
            theWriter.Flush();
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public void Disconnect()
    {
        try
        {
            if (!socketReady)
                return;
            theWriter.Write("");
            theWriter.Flush();
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public byte[] ReadSocket(int maxBuffer = 8192)
    {
        try
        {
            byte[] buffer = new byte[maxBuffer];
            int bytesRead = 0;
            int chunk;

            chunk = theStream.Read(buffer, (int)bytesRead, buffer.Length - (int)bytesRead);
            if (chunk == 0)
            {
                socketReady = false;
                throw new SocketException();
            }
            bytesRead += chunk; 
            return buffer;
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
        return null;
    }

    public void CloseSocket()
    {
        if (!socketReady)
            return;
        theWriter.Close();
        theReader.Close();
        client.Close();
        socketReady = false;
    }
}


