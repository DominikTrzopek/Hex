using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterNoise : MonoBehaviour
{
    public float power = 3;
    public float scale = 1;
    public float timescale = 1;
    public float offset_x, offset_y;
    MeshFilter mesh;
    void Start()
    {
        mesh = GetComponent<MeshFilter>();

    }

    void Update()
    {
        GenerateNoise();
        set = true;
        if (offset_y <= 0.1) 
            offset_y += Time.deltaTime * timescale;
        if (offset_y >= power)
            offset_y -= Time.deltaTime * timescale;
        offset_x += Time.deltaTime * timescale;
    }
    Vector3[] vertices;
    bool set = false;
    void GenerateNoise()
    {
        if(set == false)
            vertices = mesh.mesh.vertices;
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * power;
        }
        mesh.mesh.vertices = vertices;
    }

    float x_cord, y_cord;

    float CalculateHeight(float x, float y)
    {
        x_cord = x * scale + offset_x;
        y_cord = y * scale + offset_y;
        return Mathf.PerlinNoise(x_cord, y_cord);
    }
}
