using UnityEngine;

[System.Serializable]
public class GetServerListRequest : IUDPREquest
{
    public int id;
    public RequestType requestType = RequestType.GET;

    public GetServerListRequest(int id){
        this.id = id;
    }

    public string getRequestType(){
        return requestType.ToString();
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
