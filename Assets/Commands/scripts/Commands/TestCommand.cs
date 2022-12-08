using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestCommand : ICommand
{
    string name;
    Vector2 position;
    int rotation;

    public TestCommand()
    {

    }

    public void Execute()
    {

    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public ICommand FromString(string json)
    {
        return JsonUtility.FromJson<TestCommand>(json);
    }

}
