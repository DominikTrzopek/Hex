﻿using UnityEngine;
using System.Collections.Generic;

public class CustomTag : MonoBehaviour
{
    [SerializeField]
    private List<CellTag> tags = new List<CellTag>();
    public PathTag pathTag = PathTag.none;
    public bool taken;
    public int range = 1000;
    public Vector2Int coordinates;

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
    tree,
    ore
}

public enum PathTag
{
    none,
    inRange
}