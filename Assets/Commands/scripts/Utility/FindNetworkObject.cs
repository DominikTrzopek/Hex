using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNetworkObject : MonoBehaviour
{
    public static GameObject FindObj(string objectId)
    {
        NetworkId[] scripts = Object.FindObjectsOfType<NetworkId>();
        foreach (NetworkId script in scripts)
        {
            if (script.gameObject.GetComponent<NetworkId>().objectId == objectId)
                return script.gameObject;
        }
        return null;
    }

    public static List<GameObject> FindAllNetObj(string playerId)
    {
        List<GameObject> networkObjects = new List<GameObject>();
        NetworkId[] scripts = Object.FindObjectsOfType<NetworkId>();
        foreach (NetworkId script in scripts)
        {
            if (script.gameObject.GetComponent<NetworkId>().ownerId == playerId)
                networkObjects.Add(script.transform.root.gameObject);
        }
        return networkObjects;
    }
}
