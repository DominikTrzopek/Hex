using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CustomMapLogic
{
    public static Texture2D Load(string filePath)
    {

        if(Application.isEditor)
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

    ////////////////////////
    public static float[] resize(Texture2D tex, int size)
    {
        float[] newPixels = new float[size * size];
        Color[] pixels = tex.GetPixels(0);

        int width = tex.width;
        int height = tex.height;

        //image bigger than map size
        int scaleX = width / size;
        int scaleY = height / size;

        int chunkCount = 0;
        int pixelCount = 0;
        int rowCount = 0;
        int chunkSize = scaleX * scaleY;
        Color[] chunk = new Color[chunkSize];

        int num = 0;
        int iter = 0;

        while (rowCount * scaleY + scaleY < height)
        {
            while (num != chunkSize)
            {
                for (int i = 0; i < scaleX; i++)
                {
                    chunk[num] = pixels[iter];
                    num++;
                    iter++;
                }
                iter += width - scaleX;
            }
            num = 0;
            if (pixelCount >= size * size)
            {
                return newPixels;
            }
            newPixels[pixelCount] = calculateAvgInGrayScale(chunk, chunkSize);
            pixelCount++;
            chunkCount++;

            if (chunkCount == size)
            {
                rowCount++;
                chunkCount = 0;
            }
            iter = rowCount * scaleY * width + chunkCount * scaleX;
        }
        return newPixels;
    }

    public static float[] scaleDownX(Texture2D tex, int size)
    {
        float[] newPixels = new float[size * size];
        Color[] pixels = tex.GetPixels(0);

        int width = tex.width;
        int height = tex.height;

        //image bigger than map size
        int scaleX = width / size;
        int scaleY = height / size;

        int pixelCount = 0;
        int rowCount = 0;
        int chunkSize = scaleX;
        Color[] chunk = new Color[chunkSize];

        int iter = 0;

        while (rowCount < height)
        {
            do
            {
                for (int i = 0; i < getChunkSize(scaleX, width, iter % width); i++)
                {
                    chunk[i] = pixels[iter];
                    iter++;
                }
                Debug.Log(pixelCount);
                try
                {
                    newPixels[pixelCount] = calculateAvgInGrayScale(chunk, chunkSize);
                    pixelCount++;
                }
                catch
                {
                    return newPixels;
                }
            } while (iter % width != 0);
            rowCount++;
        }
        return newPixels;
    }

    private static int getChunkSize(int scaleX, int width, int iter)
    {
        return Math.Min(scaleX, width - iter);
    }

    private static float calculateAvgInGrayScale(Color[] arr, int chunkSize)
    {
        float r = 0, g = 0, b = 0;
        foreach (Color pixel in arr)
        {
            //  Debug.Log(pixel.r);
            r += pixel.r;
            g += pixel.g;
            b += pixel.b;
        }
        return (r + g + b) / (chunkSize * 3f);
    }
    /////////////////////////////

    public static Texture2D scaled(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
    {
        Rect texR = new Rect(0, 0, width, height);
        _gpu_scale(src, width, height, mode);

        //Get rendered data back to a new texture
        Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true);
        result.Reinitialize(width, height);
        result.ReadPixels(texR, 0, 0, true);
        //result.SetPixels(src.GetPixels(),0);
        return result;
    }

    static void _gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
    {
        //We need the source texture in VRAM because we render with it
        src.filterMode = fmode;
        src.Apply(true);

        //Using RTT for best quality and performance. Thanks, Unity 5
        RenderTexture rtt = new RenderTexture(width, height, 32);

        //Set the RTT in order to render to it
        Graphics.SetRenderTarget(rtt);

        //Setup 2D matrix in range 0..1, so nobody needs to care about sized
        GL.LoadPixelMatrix(0, 1, 1, 0);

        //Then clear & draw the texture to fill the entire RTT.
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
    }

    public static float[] getGreyScale(Texture2D tex, int size)
    {
        float[] newPixels = new float[size * size];
        int i = 0;
        foreach (Color pixel in tex.GetPixels())
        {
            newPixels[i] = (pixel.r + pixel.g + pixel.b) / 3f;
            i++;
        }
        return newPixels;
    }

    public static System.Int16[] convertToTerrainLevelMap(float[] map, int levels)
    {
        System.Int16[] result = new System.Int16[map.Length];
        for(int i = 0; i < map.Length; i++)
        {
            result[i] = (System.Int16)(map[i] * levels);
        }
        return result;
    }

    public static float[] convertToNoiseMap(System.Int16[] map, int levels)
    {
        float[] result = new float[map.Length];
        float level = 1f / levels;
        for(int i = 0; i < map.Length; i++)
        {
            result[i] = map[i] * level;
        }
        return result;
    }

    public static System.Int16[] compressData(System.Int16[] data)
    {
        List<System.Int16> tmp = new List<System.Int16>();
        System.Int16 count = 0;
        System.Int16 val = data[0];
        for(int i = 1; i < data.Length; i++)
        {   
            if(data[i - 1] == data[i])
            {
                count++;
            }
            else
            {
                count++;
                tmp.Add(val);
                tmp.Add(count);

                count = 0;
                val = data[i];
            }
        }
        count++;
        tmp.Add(val);
        tmp.Add(count);

        System.Int16[] result = new System.Int16[tmp.Count];
        for(int i = 0; i < tmp.Count; i++)
        {
            result[i] = tmp[i];
        }
        return result;
    }

    public static System.Int16[] decompressData(System.Int16[] data)
    {
        int dataLength = 0;
        for(int i = 0; i < data.Length - 1; i += 2)
        {
            dataLength += data[i + 1];
        }
        System.Int16[] result = new System.Int16[dataLength];

        int iter = 0;
        for(int i = 0; i < data.Length - 1; i += 2)
        {   
            for(int j = 0; j < data[i + 1]; j++)
            {
                result[iter] = data[i];
                iter++;
            }

        }
        return result;
    }



}
