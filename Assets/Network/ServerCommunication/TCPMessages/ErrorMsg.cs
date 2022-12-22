using UnityEngine;

[System.Serializable]
public class ErrorMsg
{
    public ResponseType code;
    public string errorMessage;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}
