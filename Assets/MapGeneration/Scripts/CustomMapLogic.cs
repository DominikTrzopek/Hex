
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CustomMapLogic
{
    public static Texture2D Load(string filePath)
    {

        if (Application.isEditor)
            filePath = Application.dataPath + "/Resources/Maps/" + filePath;

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        else
        {
            throw new FileNotFoundException();
        }
        return tex;
    }

    public static Texture2D Scale(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
    {
        Rect texR = new Rect(0, 0, width, height);
        GpuScale(src, width, height, mode);

        Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true);
        result.Reinitialize(width, height);
        result.ReadPixels(texR, 0, 0, true);
        return result;
    }

    static void GpuScale(Texture2D src, int width, int height, FilterMode fmode)
    {
        src.filterMode = fmode;
        src.Apply(true);

        RenderTexture rtt = new RenderTexture(width, height, 32);
        Graphics.SetRenderTarget(rtt);

        GL.LoadPixelMatrix(0, 1, 1, 0);
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
    }

    public static float[] GetGreyScale(Texture2D tex, int size)
    {
        float[] newPixels = new float[size * size];
        int i = 0;
        foreach (Color pixel in tex.GetPixels())
        {
            newPixels[i] = pixel.grayscale;
            i++;
        }
        return newPixels;
    }

    public static System.Int16[] ConvertToTerrainLevelMap(float[] map, int levels)
    {
        System.Int16[] result = new System.Int16[map.Length];
        for (int i = 0; i < map.Length; i++)
        {
            result[i] = (System.Int16)(map[i] * levels);
        }
        return result;
    }

    public static float[] ConvertToNoiseMap(System.Int16[] map, int levels)
    {
        float[] result = new float[map.Length];
        float level = 1f / levels;
        for (int i = 0; i < map.Length; i++)
        {
            result[i] = map[i] * level;
        }
        return result;
    }

    public static string[] CompressData(System.Int16[] data)
    {
        List<string> tmp = new List<string>();
        System.Int16 count = 0;
        System.Int16 val = data[0];
        for (int i = 1; i < data.Length; i++)
        {
            count++;
            if (data[i - 1] != data[i])
            {
                AddToList(tmp, val, count);
                count = 0;
                val = data[i];
            }
        }
        count++;
        AddToList(tmp, val, count);

        string[] result = new string[tmp.Count];
        for (int i = 0; i < tmp.Count; i++)
        {
            result[i] = tmp[i];
        }
        return result;
    }

    private static void AddToList(List<string> tmp, System.Int16 val, System.Int16 count)
    {
        if(count > 1)
            tmp.Add(val + ":" + count);
        else
            tmp.Add(val.ToString());
    }

    public static System.Int16[] DecompressData(string str)
    {
        string[] data = str.Split(",");
        
        Int32 dataLength = 0;
        for (int i = 0; i < data.Length; i += 1)
        {
            string[] splited = data[i].Split(":");
            if(splited.Length == 1)
                dataLength += 1;
            else
                dataLength += Int32.Parse(splited[1]);
        }
        System.Int16[] result = new System.Int16[dataLength];

        int iter = 0;
        for (int i = 0; i < data.Length; i += 1)
        {
            int lenght;
            string[] splited = data[i].Split(":");
            if(splited.Length == 1)
                lenght = 1;
            else
                lenght = Int16.Parse(splited[1]);
            for (int j = 0; j < lenght; j++)
            {
                result[iter] = Int16.Parse(splited[0]);
                iter++;
            }
        }
        return result;
    }

    public static string SerializeToString(string[] array)
    {
        string result = "";
        foreach(string elem in array)
        {
            result += elem + ",";
        }
        return result.Remove(result.Length - 1);
    }

}
