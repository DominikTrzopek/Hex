
using UnityEngine;

public static class UDPServerConfig
{
    private static string ipAddress = "192.168.0.219";
    private static int port = 8051;
    private static string playerName = "player";
    private static string id = System.Guid.NewGuid().ToString().Substring(0, 8);
    private static string secretId = System.Guid.NewGuid().ToString();
    private static string secretHash;

    public static int GetPort()
    {
        return port;
    }

    public static string GetIp()
    {
        return ipAddress;
    }

    public static string GetPlayerName()
    {
        return playerName;
    }

    public static void SetPort(int newPort)
    {
        port = newPort;
    }

    public static void SetIP(string newIp)
    {
        ipAddress = newIp;
    }

    public static void SetPlayerName(string newName)
    {
        playerName = newName;
    }

    public static string GetId()
    {
        return id;
    }

    public static string GetSecretId()
    {
        return secretId;
    }

    public static string GetSecretHash()
    {
        return secretHash;
    }

    public static void SetSecretHash(string hash)
    {
        secretHash = hash;
    }

}
