
using UnityEngine;

public class UDPServerConfig : MonoBehaviour
{
    private static string ipAddress = "192.168.0.219";
    private static int port = 8051;
    private static string playerName = "player";

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

}
