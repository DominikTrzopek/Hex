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

    public void setupSocket(String host, Int32 port)
    {
        try
        {
            client = new TcpClient(host, port);
            client.SendTimeout = 50000;
            client.ReceiveTimeout = 50000;
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
    public void writeSocket(ITCPMsg msg)
    {
        try
        {
            if (!socketReady)
                return;
            theWriter.Write(msg.saveToString() + "\n");
            theWriter.Flush();
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public byte[] readSocket(int maxBuffer = 1024)
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

    public void closeSocket()
    {
        if (!socketReady)
            return;
        theWriter.Close();
        theReader.Close();
        client.Close();
        socketReady = false;
    }
}


