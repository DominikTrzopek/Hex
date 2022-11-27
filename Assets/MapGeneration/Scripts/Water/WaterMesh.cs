using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMesh : MonoBehaviour
{

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public int size_x = 70 - 4;
    public int size_z = 60 - 4;

    private void CreateShape()
    {
        vertices = new Vector3[(size_x + 1) * (size_z + 1)];
        for(int i = 0, z = 0; z < (size_z + 1); z++)
        {
            for (int j = 0; j < (size_x + 1); j++)
            {
                vertices[i] = new Vector3(j, 0, z);
                i++;
            }
        }
        triangles = new int[size_x * size_z * 6];
        int vert = 0;
        int trian = 0;
        for (int z = 0; z < size_z; z++)
        {
            for (int x = 0; x < size_x; x++)
            {
                
                triangles[trian] = vert;
                triangles[trian + 1] = size_x + vert + 1;
                triangles[trian + 2] = vert + 1;
                triangles[trian + 3] = vert + 1;
                triangles[trian + 4] = size_x + vert + 1;
                triangles[trian + 5] = size_x + vert + 2;
                trian += 6;
                vert++;
            }
            vert++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }


    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

}
