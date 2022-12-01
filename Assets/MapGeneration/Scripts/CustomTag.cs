using UnityEngine;
using System.Collections.Generic;

public class CustomTag : MonoBehaviour
{
    /*-----------------------------------------------
    Spis tagow:
    0 - czy jest w zasiegu - Pathfinding, czy odwiedzony - rekurencyjny
    1 - czy jest w zasiegu - Pathfinding rekurencyjny
    2 - typ terenu, droga
    -----------------------------------------------*/
    [SerializeField]
    private List<CellTag> tags = new List<CellTag>();
    public PathTag pathTag = PathTag.none;
    public bool taken;
    public int range = 1000;
    public Vector2Int coordinates;

    
    public bool in_base_range;
    public int noise = 1;
    public bool has_tree = false;
    public bool has_ore = false;
    public bool is_safe = false;
    public float spawn_chance = 0;
    public bool spawn = false;
    public short distance_from_HQ = 1000;
    public bool taken_by_player;

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
    mainBase
}

public enum PathTag
{
    none,
    inRange

}