using UnityEngine;
using System.Collections.Generic;

public class CustomTag : MonoBehaviour
{
    [SerializeField]
    private List<CellTag> tags = new List<CellTag>();
    public PathTag pathTag = PathTag.none;
    public bool taken;
    public int range = 1000;
    public Vector2Int coordinates;
    public bool active;
    public bool getResources;

    public bool HasTag(CellTag tag)
    {
        return tags.Contains(tag);
    }

    public IEnumerable<CellTag> GetTags()
    {
        return tags;
    }

    public void Rename(int index, CellTag tagName)
    {
        tags[index] = tagName;
    }

    public void Add(CellTag tagName)
    {
        tags.Add(tagName);
    }

    public CellTag GetAtIndex(int index)
    {
        return tags[index];
    }

    public int Count
    {
        get { return tags.Count; }
    }
}

public enum CellTag
{
    water,
    standard,
    mountain,
    structure,
    obstruction,
    mainBase,
    building,
    tree,
    ore,
    player
}

public enum PathTag
{
    none,
    inRange
}