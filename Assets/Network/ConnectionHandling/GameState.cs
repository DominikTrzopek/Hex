using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
    public List<NetworkObjInfo> networkInfos;

    public GameState()
    {
        this.networkInfos = SaveGameState();
    }

    public GameState(bool enable)
    {
        try
        {
            this.networkInfos.Clear();
        }
        catch { }
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public static GameState fromString(string json)
    {
        return JsonUtility.FromJson<GameState>(json);
    }

    public List<NetworkObjInfo> SaveGameState()
    {
        List<NetworkObjInfo> networkInfos = new List<NetworkObjInfo>();
        List<GameObject> objs = FindNetworkObject.FindAllNetObj();
        foreach (GameObject obj in objs)
        {
            if (obj.GetComponent<UnitStats>())
            {
                networkInfos.Add(
                    new NetworkObjInfo(
                        obj.name,
                        obj.GetComponent<NetworkId>().ownerId,
                        obj.GetComponent<NetworkId>().objectId,
                        obj.GetComponent<NetworkId>().position,
                        obj.GetComponent<UnitStats>().GetHP(),
                        obj.GetComponent<UnitStats>().GetLevel(),
                        obj.GetComponent<UnitStats>().GetVR(),
                        0,
                        obj.GetComponent<UnitStats>().GetAP(),
                        obj.GetComponent<UnitStats>().GetMR(),
                        obj.GetComponent<UnitStats>().GetAR(),
                        ""
                    )
                );
            }
            if (obj.GetComponent<StructureStats>())
            {
                networkInfos.Add(
                    new NetworkObjInfo(
                        obj.name,
                        obj.GetComponent<NetworkId>().ownerId,
                        obj.GetComponent<NetworkId>().objectId,
                        obj.GetComponent<NetworkId>().position,
                        obj.GetComponent<StructureStats>().GetHP(),
                        obj.GetComponent<StructureStats>().GetLevel(),
                        obj.GetComponent<StructureStats>().GetVR(),
                        obj.GetComponent<StructureStats>().GetTakenRange(),
                        0,
                        0,
                        0,
                        obj.GetComponent<StructureStats>().parentId
                    )
                );
            }
        }
        return networkInfos;
    }

}
