using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateServerRequest : IUDPREquest
{
    public int id; //ToDo: get player id
    public string requestType = "create";
    public string password;
    public int numberOfPlayers;
    public int numberOfTurns;
    public int seed = Random.Range(-10000,10000);
    public int mapType; //ToDo: change to enum

    public CreateServerRequest(int id, string password, int numberOfPlayers, int numberOfTurns, int mapType){
        this.id = id;
        this.password = password;
        this.numberOfPlayers = numberOfPlayers;
        this.numberOfTurns = numberOfPlayers;
        this.mapType = mapType;
    }

    public string getRequestType(){
        return requestType;
    }

    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }

}
