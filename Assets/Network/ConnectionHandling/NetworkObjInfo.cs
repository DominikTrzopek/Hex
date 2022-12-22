using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetworkObjInfo
{
    public string name;
    public string ownerId;
    public string objectId;
    public Vector2Int position;
    public string parentId;

    public int HP;
    public int lv;
    public int AP;
    public int MR;
    public int AR;

    public NetworkObjInfo(string name, string ownerId, string objectId, Vector2Int position, int HP, int lv, int AP, int MR, int AR)
    {
        this.name = name;
        this.ownerId = ownerId;
        this.objectId = objectId;
        this.position = position;
        this.HP = HP;
        this.lv = lv;
        this.AP = AP;
        this.MR = MR;
        this.AR = AR;
    }

    public NetworkObjInfo(string name, string ownerId, string objectId, Vector2Int position, int HP, int lv, string parentId)
    {
        this.name = name;
        this.ownerId = ownerId;
        this.objectId = objectId;
        this.position = position;
        this.HP = HP;
        this.lv = lv;
        this.parentId = parentId;
    }
}
