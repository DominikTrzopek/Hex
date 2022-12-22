using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkId : MonoBehaviour
{
    public string ownerId;
    public string objectId;
    public Vector2Int position;

    public void SetIds(string ownerId, string objectId)
    {
        this.ownerId = ownerId;
        this.objectId = objectId;
    }

}
