using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainType
{
    public const int levels = 10;

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
            { "taiga", TerrainEnum.polar }
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
public class Biome
{
    public Terrain[] terrains = new Terrain[TerrainType.levels];
    public TerrainEnum name;
    public Color standardColor;
    public int falloff;
    public float persistance;
    public float lacunarity;
    public float scale;
    public int octaves;
    public float instantiateHeight;
    public float treeDensity;
    public float oreDensity;
    public GameObject tree;
    public AnimationCurve animationCurve;
}