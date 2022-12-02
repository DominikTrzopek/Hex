using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkId : MonoBehaviour
{
    public string ownerId;
    public string objectId;

    public void setIds(string ownerId, string objectId)
    {
        this.ownerId = ownerId;
        this.objectId = objectId;
    }

}
