using UnityEngine;

public class MakeServerRequest : MonoBehaviour
{

    public void RequestNewGameServer()
    {
        UDPClient client = new UDPClient();
        client.init();
        client.sendData(new CreateServerRequest(1,"pass",3,4,5));
    }

    public void RequestServerList()
    {
        UDPClient client = new UDPClient();
        client.init();
        client.sendData(new GetServerListRequest(1));
    }
    


}
