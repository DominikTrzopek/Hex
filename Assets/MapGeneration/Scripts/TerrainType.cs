using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainType
{

    public static Biome initTerrainType(TerrainEnum type, Biome[] biomes)
    {
        foreach(Biome biom in biomes)
        {
            if(biom.name == type)
            {
                return biom;
            }
        }
        return biomes[0];
    }

    public static TerrainEnum mapTerrainEnum(string value)
    {
        var dict = new Dictionary<string, TerrainEnum> {
            { "plains", TerrainEnum.plains},
            { "island", TerrainEnum.island },
            { "mountains", TerrainEnum.mountains },
            { "desert", TerrainEnum.desert },
            { "canyon", TerrainEnum.canyon},
            { "oasis", TerrainEnum.oasis },
            { "swamp", TerrainEnum.swamp },
            { "taiga", TerrainEnum.taiga }
        };
        return dict[value];
    }

}

[System.Serializable]
public struct Terrain
{
    public CellTag name;
    public Color regionColour;

    public Terrain(CellTag name, Color color)
    {
        this.name = name;
        this.regionColour = color;
    }
}

[System.Serializable]
public struct Biome
{
    public Terrain[] terrains;
    public TerrainEnum name;
    public int falloff;
}