using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
    public int width;
    public int height;
    public MeshData GenerateTerrain()
    {


        float topLeftX = (width - 1) / -2f;
        float topLeftY = (height - 1) / 2f;
        int vertexIndex = 0;
        int verticlePerLine = width;
        MeshData meshData = new MeshData(verticlePerLine, verticlePerLine);
        for (int y = 0; y < height; y ++)
        {
            for (int x = 0; x < width; x ++)
            {
                meshData.verticles[vertexIndex] = new Vector3(topLeftX + x, 0, topLeftY - y);
                //Debug.Log(heightMap[x, y]);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticlePerLine + 1, vertexIndex + verticlePerLine);
                    meshData.AddTriangle(vertexIndex + 1 + verticlePerLine, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
        meshData.Flatshading();
        return meshData;
    }

    Mesh mesh;
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        MeshData meshdata = GenerateTerrain();
        mesh.vertices = meshdata.verticles;
        mesh.triangles = meshdata.triangles;
        mesh.uv = meshdata.uvs;
        mesh.RecalculateNormals();
    }
}
public class MeshData
{
    public Vector3[] verticles;
    public int[] triangles;
    int triangleIndex;
    public Vector2[] uvs;
    //bool flatshading;
    public MeshData(int mapWidth, int mapHeight)
    {
        verticles = new Vector3[mapWidth * mapHeight];
        uvs = new Vector2[mapWidth * mapHeight];
        triangles = new int[(mapHeight - 1) * (mapWidth - 1) * 6];
    }
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticles;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
       // if (flatshading)
       //    mesh.RecalculateNormals();
        return mesh;
    }

    public void Flatshading()
    {
        Vector3[] flatshadeing_verticle = new Vector3[triangles.Length];
        Vector2[] flatshading_uvs = new Vector2[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            flatshadeing_verticle[i] = verticles[triangles[i]];
            flatshading_uvs[i] = uvs[triangles[i]];
            triangles[i] = i;
        }
        verticles = flatshadeing_verticle;
        uvs = flatshading_uvs;
    }

}