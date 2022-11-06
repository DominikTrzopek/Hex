using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
public class TCPClient
{
    internal Boolean socketReady = false;
    TcpClient client;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;

    public void setupSocket(String host, Int32 port)
    {
        try
        {
            client = new TcpClient(host, port);
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
    public void writeSocket(string theLine)
    {
        if (!socketReady)
            return;
        String foo = theLine + "\r\n";
        theWriter.Write(foo);
        theWriter.Flush();
    }
    public String readSocket()
    {
        if (!socketReady)
            return "";
        if (theStream.DataAvailable)
            return theReader.ReadLine();
        return "";
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
} // end class s_TCP


