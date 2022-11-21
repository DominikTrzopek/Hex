
using UnityEngine;

public static class UDPServerConfig
{
    private static string ipAddress = "192.168.0.219";
    private static int port = 8051;
    private static string playerName = "player";
    private static string id = System.Guid.NewGuid().ToString(); //SystemInfo.deviceUniqueIdentifier;
    private static string secretId = System.Guid.NewGuid().ToString();

    public static int getPort()
    {
        return port;
    }

    public static string getIp()
    {
        return ipAddress;
    }

    public static string getPlayerName()
    {
        return playerName;
    }

    public static void setPort(int newPort)
    {
        port = newPort;
    }

    public static void setIP(string newIp)
    {
        ipAddress = newIp;
    }

    public static void setPlayerName(string newName)
    {
        playerName = newName;
    }

    public static string getId()
    {
        return id;
    }

    public static string getSecretId()
    {
        return secretId;
    }

}
