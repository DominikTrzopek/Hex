using UnityEngine;

[System.Serializable]
public class UDPResponse : IUDPResponse
{
    public TCPServerInfo serverInfo;
    public ResponseType responseType;
    
    public string GetResponseCode(){
        return responseType.ToString();
    }
    public static UDPResponse FromString(string json){
        return JsonUtility.FromJson<UDPResponse>(json);
    }
    
}
