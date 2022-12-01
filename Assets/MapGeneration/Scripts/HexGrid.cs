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
    public GameObject basePrefab;
    public Biome[] terrain;
    public int usefalloff = 1;
    public float instantiate_height = 4;

    public GameObject[] stone;
    public GameObject bridge;
    public GameObject ore;
    public float chance;
    public static GameObject[,] hex_array;


    [SerializeField]
    private Biome[] biomes;

    private float fixedYPosition;
    private Biome biome;


    void CreateMap(int size, TerrainEnum biomName, bool useCustomMap)
    {
        biome = TerrainType.initTerrainType(biomName, biomes);
        Terrain[] terrain = biome.terrains;
        hex_array = new GameObject[size, size];
        seed = UnityEngine.Random.Range(-1000,1000);
        int levels = terrain.Length;
        float[,] heightMap = new float[size, size];

        ///
        Texture2D level = CustomMapLogic.Load("map.jpg");
        
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
            heightMap = Noise.GenerateFromCustomMap(new_val, size, levels, biome.falloff);
        }
        else
        {
            heightMap = Noise.GenerateNoiseMap(size, scale, persistance, lacunarity, octaves, seed, offset, levels, biome.falloff);
        }

        Vector3 position = new Vector3(0, 0, 0);
        Quaternion rotation = hex.transform.rotation;
        float level_height = 1f / levels;

        int tree_seed = seed + 1;
        float[,] tree_map = new float[size, size];
        tree_map = Noise.GenerateNoiseMap(size, scale / 2, persistance, lacunarity, octaves, tree_seed, offset, levels, 0);

        int ore_seed = seed + 2;
        float[,] ore_map = new float[size, size];
        ore_map = Noise.GenerateNoiseMap(size, scale / 5, persistance / 5, lacunarity / 5, octaves * 2, ore_seed, offset, 100, 0);

        int stoneCount = 0;

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                position.x = ((x + z * 0.5f - z / 2) * (HexMetrics.innerRadious * 2f)) * 2;
                position.y = animationCurve.Evaluate(heightMap[x, z]) * multiplayer;
                position.z = z * (HexMetrics.outerRadious * 1.5f);

                GameObject obj = Instantiate(hex, position, rotation);
                hex_array[x, z] = obj;
                obj.GetComponent<CustomTag>().coordinates = new Vector2Int(x,z);

                if(heightMap[x,z] >= level_height * instantiate_height)
                {
                    
                    for (int l = 0; l < levels; l++)
                    {
                        if (heightMap[x, z] <= (level_height * l) + level_height)
                        {

                            obj.GetComponent<Renderer>().material.color = terrain[l].regionColour;
                            obj.GetComponent<CustomTag>().Rename(0, terrain[l].name);
                            break;
                        }
                    }

                    if(obj.GetComponent<CustomTag>().HasTag(CellTag.standard))
                    {
                        if (tree_map[x, z] >= 0.7f)
                        {
                            obj = SpawnBase.replaceTile(obj, tree, obj.GetComponent<Renderer>().material.color, obj.transform.position.y);
                            obj.GetComponent<CustomTag>().has_tree = true;
                            hex_array[x, z] = obj;
                        }

                        else if (ore_map[x, z] <= chance)
                        {
                            if(stoneCount == 3)
                            {
                                obj = SpawnBase.replaceTile(obj, ore, obj.GetComponent<Renderer>().material.color, obj.transform.position.y);  
                                stoneCount = 0;
                            }
                            else
                            {
                                obj = SpawnBase.replaceTile(obj, stone[0], obj.GetComponent<Renderer>().material.color, obj.transform.position.y);
                                obj.GetComponent<CustomTag>().taken = true;
                                obj.GetComponent<CustomTag>().has_ore = true;
                                hex_array[x, z] = obj;
                                stoneCount++;
                            }
                        }
                    }
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().enabled = false;
                    obj.GetComponent<CustomTag>().Rename(0, terrain[0].name);
                }
            }
        }
    }
    void Start()
    {
        //Application.targetFrameRate = 60;

        int numOfPlayers = 4;
        
        CreateMap(size, TerrainType.mapTerrainEnum("island"), false);
        fixedYPosition = animationCurve.Evaluate(0.2f) * multiplayer;
        float padding = usefalloff != 0 ? 0.25f : 0.15f;
        int border = (int)(size * padding);
        if(!SpawnBase.prepareGrid(size, numOfPlayers, basePrefab, hex, ore, fixedYPosition , biome, usefalloff != 0 ? 0.25f : 0.15f))
        {
            for (int x = border; x < size - border; x++)
            {
                for (int z = border; z < size - border; z++)
                {
                    if((z == x || z == x + 1) && !hex_array[x,z].GetComponent<CustomTag>().HasTag(CellTag.standard))
                    {
                        hex_array[x,z] = SpawnBase.replaceTile(hex_array[x,z], bridge, hex_array[x,z].GetComponent<Renderer>().material.color,  fixedYPosition);
                    }

                    if(numOfPlayers > 2)
                    {
                        if ((z + x == size - 1 || z + x == size - 2) && !hex_array[x,z].GetComponent<CustomTag>().HasTag(CellTag.standard)) 
                        {
                            hex_array[x,z] = SpawnBase.replaceTile(hex_array[x,z], bridge, hex_array[x,z].GetComponent<Renderer>().material.color,  fixedYPosition);
                        }
                    }


                }

            }
        }
    }


}
