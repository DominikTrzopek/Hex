using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour
{
    public GameObject errorPannel;
    public static bool reconnected = false;
    void Update()
    {
        StartCoroutine(Coroutine());
        if (TCPConnection.instance.client.socketReady)
        {
            Debug.Log("Connected");
            errorPannel.SetActive(false);
        }
        else
        {
            errorPannel.SetActive(true);
        }

        if (Input.GetKey("q"))
        {
            TCPConnection.instance.client.Disconnect();
            TCPConnection.instance.client.CloseSocket();
        }
    }

    public void Reconnect()
    {
        reconnected = true;
        TCPConnection.instance.Reconnect();
    }

    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(1);
    }

    public static void Reinstantiate(GameState state)
    {
        foreach (NetworkObjInfo obj in state.networkInfos)
        {
            Debug.Log(obj.name);
            GameObject found = FindNetworkObject.FindObj(obj.objectId);
            if (found != null)
            {
                UpdateObj(found, obj);
            }
            else
            {
                Vector3 position = HexGrid.hexArray[obj.position.x, obj.position.y].transform.position;
                GameObject newObj = null;
                if (obj.name == "tank(Clone)")
                {
                    newObj = Instantiate(PrefabContainer.container.playerUnitPrefab[0], position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    HexGrid.hexArray[obj.position.x, obj.position.y].GetComponent<CustomTag>().taken = true;
                    Color color = PlayerInfoGetter.GetColor(obj.ownerId);
                    foreach (Transform child in newObj.GetComponentsInChildren<Transform>())
                    {
                        try
                        {
                            child.GetComponent<Renderer>().material.color = color;
                        }
                        catch { }
                    }
                    newObj.GetComponent<StatsAbstract>().SetValues(obj.lv, obj.HP);
                    newObj.GetComponent<UnitStats>().UpdateUnit(obj.AP, obj.MR, obj.AR);
                }
                if (obj.name == "structure(Clone)")
                {
                    newObj = Instantiate(PrefabContainer.container.structurePrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    newObj.GetComponent<TakeCell>().MarkCells(obj.ownerId);
                    newObj.GetComponent<StatsAbstract>().SetValues(obj.lv, obj.HP);
                    newObj.GetComponent<StructureStats>().parentId = obj.parentId;
                    GameObject hex = HexGrid.hexArray[obj.position.x, obj.position.y];

                    CustomTag tags = hex.GetComponent<CustomTag>();
                    tags.Rename(0, CellTag.obstruction);
                    if (tags.HasTag(CellTag.tree))
                    {
                        Object.Destroy(hex.transform.GetChild(2).gameObject);
                        tags.Rename(1, CellTag.structure);
                    }
                    else
                        tags.Add(CellTag.structure);

                }
                newObj.GetComponent<NetworkId>().ownerId = obj.ownerId;
                newObj.GetComponent<NetworkId>().objectId = obj.objectId;
                newObj.GetComponent<NetworkId>().position = obj.position;
            }
        }

        List<GameObject> allExisting = FindNetworkObject.FindAllNetObj();
        for (int i = 0; i < allExisting.Count; i++)
        {
            bool todelete = true;
            int count = 0;
            foreach (NetworkObjInfo obj in state.networkInfos)
            {
                if (obj.objectId == allExisting[i].GetComponent<NetworkId>().objectId)
                {
                    todelete = false;
                    count++;
                    if(count > 1)
                    {
                        Object.Destroy(allExisting[i]);
                        count--;
                    }
                }
            }
            if (todelete)
            {
                Object.Destroy(allExisting[i]);
                continue;
            }

            if(allExisting[i].GetComponent<StructureStats>())
            {
                string parentId = allExisting[i].GetComponent<StructureStats>().parentId;
                GameObject parent = FindNetworkObject.FindObj(parentId);
                parent.GetComponent<StructureStats>().AddConnected(allExisting[i]);
            }

        }

        

    }

    public static void UpdateObj(GameObject obj, NetworkObjInfo info)
    {
        GameObject hex = HexGrid.hexArray[info.position.x, info.position.y];
        hex.GetComponent<CustomTag>().taken = true;
        Vector3 position = hex.transform.position;
        obj.transform.position = position;
        obj.GetComponent<NetworkId>().position = info.position;
        obj.GetComponent<StatsAbstract>().SetValues(info.lv, info.HP);
    }

    public static void UpdateUnit(GameObject obj, NetworkObjInfo info)
    {
        obj.GetComponent<UnitStats>().UpdateUnit(info.AP, info.MR, info.AR);
    }

}
