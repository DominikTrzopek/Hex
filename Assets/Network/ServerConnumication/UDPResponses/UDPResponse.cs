using UnityEngine;

[System.Serializable]
public class UDPResponse : IUDPResponse
{
    public TCPServerInfo serverInfo;
    public ResponseType responseType;
    
    public string getResponseCode(){
        return responseType.ToString();
    }
    public static UDPResponse fromString(string json){
        return JsonUtility.FromJson<UDPResponse>(json);
    }
    
}
