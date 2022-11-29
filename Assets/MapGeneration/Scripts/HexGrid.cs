using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HexGrid : MonoBehaviour
{
    public int size = 24;
    public GameObject hex, tree;
    public float scale, persistance, lacunarity;
    public int octaves, seed;
    public AnimationCurve animationCurve;
    public float multiplayer = 1f;
    Vector2 offset = new Vector2(1, 1);
    public Biome[] terrain;
    public int usefalloff = 1;
    public float instantiate_height = 4;

    public GameObject[] ore;
    public float chance;
    public static GameObject[,] hex_array;


    [SerializeField]
    private Biome[] biomes;


    void CreateMap(int size, TerrainEnum biomName, bool useCustomMap)
    {
        Biome biom = TerrainType.initTerrainType(biomName, biomes);
        Terrain[] terrain = biom.terrains;
        hex_array = new GameObject[size, size];
        seed = UnityEngine.Random.Range(-10000, 10000);
        int levels = terrain.Length;
        float[,] heightMap = new float[size, size];

        ///
        Texture2D level = CustomMapLogic.Load("dd.jpg");
        
        level = CustomMapLogic.scaled(level, size, size);
        float[] val = CustomMapLogic.getGreyScale(level, size);

        //float[] val = CustomMapLogic.resize(level, size);

        ///
        ///
        int[] levelMap = CustomMapLogic.convertToTerrainLevelMap(val, levels);
        int[] test = CustomMapLogic.compressData(levelMap);
        int[] test2 = CustomMapLogic.decompressData(test);
        float[] new_val = CustomMapLogic.convertToNoiseMap(levelMap,levels);
        ///

        CustomMapLogic.convertToNoiseMap(levelMap, levels);
        if(useCustomMap)
        {
            heightMap = Noise.GenerateFromCustomMap(new_val, size, levels, biom.falloff);
        }
        else
        {
            heightMap = Noise.GenerateNoiseMap(size, scale, persistance, lacunarity, octaves, seed, offset, levels, biom.falloff);
        }

        Vector3 position = new Vector3(0, 0, 0);
        Quaternion rotation = hex.transform.rotation;
        float level_height = 1f / levels;

        int tree_seed = UnityEngine.Random.Range(-10000, 10000);
        float[,] tree_map = new float[size, size];
        tree_map = Noise.GenerateNoiseMap(size, scale / 2, persistance, lacunarity, octaves, tree_seed, offset, levels, 0);

        int ore_seed = UnityEngine.Random.Range(-10000, 10000);
        float[,] ore_map = new float[size, size];
        ore_map = Noise.GenerateNoiseMap(size, scale / 5, persistance / 5, lacunarity / 5, octaves * 2, ore_seed, offset, 100, 0);

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                position.x = ((x + z * 0.5f - z / 2) * (HexMetrics.inner_radious * 2f)) * 2;
                position.y = animationCurve.Evaluate(heightMap[x, z]) * multiplayer;
                position.z = z * (HexMetrics.outer_radious * 1.5f);
                if(heightMap[x,z] >= level_height * instantiate_height)
                {
                    
                    var obj = Instantiate(hex, position, rotation);
                    hex_array[x, z] = obj;
                    for (int l = 0; l < levels; l++)
                    {
                        if (heightMap[x, z] <= (level_height * l) + level_height)
                        {
                            obj.GetComponent<Renderer>().material.color = terrain[l].regionColour;
                            obj.GetComponent<CustomTag>().Rename(2, terrain[l].name);
                            break;
                        }
                    }

                    if(!obj.GetComponent<CustomTag>().HasTag("stone"))
                    {
                        if (tree_map[x, z] >= 0.7f)
                        {
                            Instantiate(tree, position, Quaternion.Euler(new Vector3(-90, UnityEngine.Random.Range(0, 90), 0)));
                            obj.GetComponent<CustomTag>().has_tree = true;
                        }

                        else if (ore_map[x, z] <= chance)
                        {
                            Instantiate(ore[0], position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 90), 0)));
                            obj.GetComponent<CustomTag>().taken = true;
                            obj.GetComponent<CustomTag>().has_ore = true;
                        }
                    }
                }
            }
        }
    }
    void Start()
    {
        //Application.targetFrameRate = 60;
        CreateMap(size, TerrainType.mapTerrainEnum("island"), true);
    }

    

}
