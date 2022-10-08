
using UnityEngine;

public class UDPServerConfig : MonoBehaviour
{
    private static string ipAddress = "192.168.0.219";
    private static int port = 8051;

    public static int getPort(){
        return port;
    }

    public static string getIp(){
        return ipAddress;
    }

    public static void setPort(int newPort){
        port = newPort;
    }

    public static void setIP(string newIp){
        ipAddress = newIp;
    }

}
