using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HexGrid : MonoBehaviour
{
    private float multiplayer = 1f;
    private Vector2 offset = new Vector2(1, 1);
    private float fixedYPosition;
    public GameObject hex;
    public GameObject basePrefab;
    public GameObject[] stone;
    public GameObject bridge;
    public GameObject ore;
    public GameObject border;
    public static GameObject[,] hexArray;

    [SerializeField]
    private Biome[] biomes;

    void CreateMap(TCPServerInfo serverInfo)
    {

        int size = serverInfo.mapSize;
        TerrainEnum biomName = TerrainType.mapTerrainEnum(serverInfo.mapType);
        Biome biome = TerrainType.initTerrainType(biomName, biomes);
        hexArray = new GameObject[size, size];
        int seed = serverInfo.seed;
        int levels = biome.terrains.Length;
        float[,] heightMap = new float[size, size];
        fixedYPosition = biome.animationCurve.Evaluate(0.2f) * multiplayer;
        int numberOfPlayers = serverInfo.numberOfPlayers;
  
        if(serverInfo.customMap != null && serverInfo.customMap.Length != 0)
        {
            System.Int16[] decompressed = CustomMapLogic.decompressData(serverInfo.customMap);
            float[] noiseMap = CustomMapLogic.convertToNoiseMap(decompressed, levels);
            heightMap = Noise.GenerateFromCustomMap(noiseMap, size, levels, biome.falloff);
        }
        else
        {
            heightMap = Noise.GenerateNoiseMap(size, biome.scale, biome.persistance, biome.lacunarity, biome.octaves, seed, offset, levels, biome.falloff);
        }

        Vector3 position = new Vector3(0, 0, 0);
        Quaternion rotation = hex.transform.rotation;
        float level_height = 1f / levels;

        int treeMapSeed = seed + 1;
        float prescaler = 2;
        float[,] treeMap = new float[size, size];
        treeMap = Noise.GenerateNoiseMap(size, biome.scale / prescaler, biome.persistance / prescaler, biome.lacunarity / prescaler, biome.octaves, treeMapSeed, offset, levels, 0);

        int oreMapSeed = seed + 2;
        prescaler = 5;
        float[,] oreMap = new float[size, size];
        oreMap = Noise.GenerateNoiseMap(size, biome.scale / prescaler, biome.persistance / prescaler, biome.lacunarity / prescaler, biome.octaves * 2, oreMapSeed, offset, 100, 0);
        int stoneCount = 0;

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                position.x = ((x + z * 0.5f - z / 2) * (HexMetrics.innerRadious * 2f)) * 2;
                position.y = biome.animationCurve.Evaluate(heightMap[x, z]) * multiplayer;
                position.z = z * (HexMetrics.outerRadious * 1.5f);

                GameObject obj = Instantiate(hex, position, rotation);
                obj.GetComponent<CustomTag>().coordinates = new Vector2Int(x,z);
                hexArray[x, z] = obj;

                if(heightMap[x,z] >= level_height * biome.instantiateHeight)
                {
                    
                    for (int l = 0; l < levels; l++)
                    {
                        if (heightMap[x, z] <= (level_height * l) + level_height)
                        {

                            obj.GetComponent<Renderer>().material.color = biome.terrains[l].regionColour;
                            obj.GetComponent<CustomTag>().Rename(0, biome.terrains[l].name);
                            break;
                        }
                    }

                    if(obj.GetComponent<CustomTag>().HasTag(CellTag.standard))
                    {
                        if (treeMap[x, z] >= biome.treeDensity)
                        {
                            obj = SpawnBase.replaceTile(obj, biome.tree, obj.GetComponent<Renderer>().material.color, obj.transform.position.y);
                            obj.GetComponent<CustomTag>().has_tree = true;
                            hexArray[x, z] = obj;
                        }

                        else if (oreMap[x, z] >= biome.oreDensity)
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
                                hexArray[x, z] = obj;
                                stoneCount++;
                            }
                        }
                    }
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().enabled = false;
                    obj.GetComponent<CustomTag>().Rename(0, biome.terrains[0].name);
                }
            }
        }

        float padding = biome.falloff != 0 ? 0.25f : 0.15f;
        int border = (int)(size * padding);
        if(!SpawnBase.prepareGrid(size, numberOfPlayers, basePrefab, hex, ore, fixedYPosition , biome, padding))
        {
            for (int x = border; x < size - border; x++)
            {
                for (int z = border; z < size - border; z++)
                {
                    if((z == x || z == x + 1) && !hexArray[x,z].GetComponent<CustomTag>().HasTag(CellTag.standard) && !hexArray[x,z].GetComponent<CustomTag>().HasTag(CellTag.structure))
                    {
                        hexArray[x,z] = SpawnBase.replaceTile(hexArray[x,z], bridge, hexArray[x,z].GetComponent<Renderer>().material.color,  fixedYPosition);
                    }

                    if(numberOfPlayers > 2)
                    {
                        if ((z + x == size - 1 || z + x == size - 2) && (!hexArray[x,z].GetComponent<CustomTag>().HasTag(CellTag.standard) && !hexArray[x,z].GetComponent<CustomTag>().HasTag(CellTag.structure))) 
                        {
                            hexArray[x,z] = SpawnBase.replaceTile(hexArray[x,z], bridge, hexArray[x,z].GetComponent<Renderer>().material.color,  fixedYPosition);
                        }
                    }
                }
            }
        }
    }
    void Start()
    {

        TCPServerInfo serverInfo1 = new TCPServerInfo
        (
            "this.creatorId = creatorId;",
            "this.serverName = serverName;",
            "this.password = password;",
            4,
            32,
            100,
            "island",
            64,
            null,
            2
        );

        //TCPServerInfo serverInfo = TCPConnection.instance.serverInfo;
        CreateMap(serverInfo1);
        MapBorders.makeBorder(64, border);
    }


}
