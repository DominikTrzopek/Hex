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
    const float baseLevel = 0.2f;
    const int oreStoneRatio = 3;

    [SerializeField]
    private Biome[] biomes;
    int stoneCount = 0;

    void CreateMap(TCPServerInfo serverInfo)
    {

        int size = serverInfo.mapSize;
        TerrainEnum biomName = TerrainType.mapTerrainEnum(serverInfo.mapType);
        Biome biome = TerrainType.initTerrainType(biomName, biomes);
        hexArray = new GameObject[size, size];
        int seed = serverInfo.seed;
        float[,] heightMap = new float[size, size];
        fixedYPosition = biome.animationCurve.Evaluate(baseLevel) * multiplayer;
        int numberOfPlayers = serverInfo.numberOfPlayers;

        heightMap = CalculateHeightMap(serverInfo, biome);

        Quaternion rotation = hex.transform.rotation;

        int prescaler = 3;
        float[,] treeMap = Noise.GenerateNoiseMap(size, biome.scale / prescaler, biome.persistance / prescaler, biome.lacunarity / prescaler, biome.octaves, seed + 1, offset, biome.terrains.Length, 0);
        prescaler = 5;
        float[,] oreMap = Noise.GenerateNoiseMap(size, biome.scale / prescaler, biome.persistance / prescaler, biome.lacunarity / prescaler, biome.octaves * 2, seed + 2, offset, 100, 0);

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                Vector3 position = GetPosition(x, z, biome.animationCurve.Evaluate(heightMap[x, z]));
                GameObject obj = Instantiate(hex, position, rotation);
                obj.GetComponent<CustomTag>().coordinates = new Vector2Int(x, z);
                hexArray[x, z] = obj;

                if (Math.Abs(heightMap[x, z]) >= (1f / biome.terrains.Length) * biome.instantiateHeight)
                {
                    if (Math.Abs(heightMap[x, z]) > 1)
                        heightMap[x, z] = 1;

                    ApplyTagAndColor(biome, obj, heightMap[x, z]);
                    if (obj.GetComponent<CustomTag>().HasTag(CellTag.standard))
                        SpawnSpecialObj(obj, biome, treeMap[x, z], oreMap[x, z]);
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().enabled = false;
                    obj.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    obj.GetComponent<CustomTag>().Rename(0, biome.terrains[0].name);
                }
            }
        }

        float padding = biome.falloff != 0 ? 0.25f : 0.15f;
        int border = (int)(size * padding);

        if (!GridManipulator.PrepareGrid(size, numberOfPlayers, basePrefab, hex, ore, fixedYPosition, biome, border))
            Patch(border, size, numberOfPlayers);
    }

    private void Patch(int border, int size, int numberOfPlayers)
    {
        for (int x = border; x < size - border; x++)
        {
            for (int z = border; z < size - border; z++)
            {
                if (z == x || z == x + 1)
                    ReplaceDiagonal(x, z);

                if (numberOfPlayers > 2 && (z + x == size - 1 || z + x == size - 2))
                    ReplaceDiagonal(x, z);
            }
        }
    }

    private void ReplaceDiagonal(int x, int z)
    {
        if (!hexArray[x, z].GetComponent<CustomTag>().HasTag(CellTag.standard) && !hexArray[x, z].GetComponent<CustomTag>().HasTag(CellTag.structure))
        {
            hexArray[x, z] = GridManipulator.ReplaceTile(hexArray[x, z], bridge, hexArray[x, z].GetComponent<Renderer>().material.color, fixedYPosition);
        }
    }

    private float[,] CalculateHeightMap(TCPServerInfo serverInfo, Biome biome)
    {
        if (serverInfo.customMap != null && serverInfo.customMap.Length != 0)
        {
            System.Int16[] decompressed = CustomMapLogic.DecompressData(serverInfo.customMap);
            float[] noiseMap = CustomMapLogic.ConvertToNoiseMap(decompressed, biome.terrains.Length);
            return Noise.GenerateFromCustomMap(noiseMap, serverInfo.mapSize, biome.terrains.Length, biome.falloff);
        }
        else
        {
            return Noise.GenerateNoiseMap(serverInfo.mapSize, biome.scale, biome.persistance, biome.lacunarity, biome.octaves, serverInfo.seed, offset, biome.terrains.Length, biome.falloff);
        }
    }

    private Vector3 GetPosition(int x, int z, float height)
    {
        return new Vector3(
            ((x + z * 0.5f - z / 2) * (HexMetrics.innerRadious * 2f)) * 2,
            height * multiplayer,
            z * (HexMetrics.outerRadious * 1.5f)
        );
    }

    private void ApplyTagAndColor(Biome biome, GameObject obj, float heighValue)
    {
        int levels = biome.terrains.Length;
        float levelHeight = 1f / levels;
        for (int l = 0; l < levels; l++)
        {
            if (Math.Abs(heighValue) <= (levelHeight * l) + levelHeight)
            {
                obj.GetComponent<Renderer>().material.color = biome.terrains[l].regionColour;
                obj.GetComponent<CustomTag>().Rename(0, biome.terrains[l].name);
                break;
            }
        }
    }
    private void SpawnSpecialObj(GameObject obj, Biome biome, float treeMapVal, float stoneMapVal)
    {
        if (treeMapVal >= biome.treeDensity)
        {
            obj = GridManipulator.ReplaceTile(obj, biome.tree, obj.GetComponent<Renderer>().material.color, obj.transform.position.y);
            return;
        }
        SpawnStone(obj, biome, stoneMapVal);
    }

    private void SpawnStone(GameObject obj, Biome biome, float stoneMapVal)
    {
        if (stoneMapVal >= biome.oreDensity)
        {
            if (stoneCount == oreStoneRatio)
            {
                obj = GridManipulator.ReplaceTile(obj, ore, obj.GetComponent<Renderer>().material.color, obj.transform.position.y);
                stoneCount = 0;
            }
            else
            {
                obj = GridManipulator.ReplaceTile(obj, stone[0], obj.GetComponent<Renderer>().material.color, obj.transform.position.y);
                stoneCount++;
            }
        }
    }

    void Start()
    {
        TCPServerInfo serverInfo = TCPConnection.instance.serverInfo;
        CreateMap(serverInfo);
        MapBorders.makeBorder(serverInfo.mapSize, border);
    }
}
