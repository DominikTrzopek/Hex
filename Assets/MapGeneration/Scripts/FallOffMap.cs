using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class FallOffMap
{
    public static float[,] FallOff(int size, bool reverse)
    {
        float[,] map = new float[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float valueI = i / (float)size * 2 - 1;
                float valueJ = j / (float)size * 2 - 1;
                float value = Mathf.Max(Mathf.Abs(valueI), Mathf.Abs(valueJ));
                map[i, j] = Evaluate(value, reverse ? EvaluateReverseFallOff : EvaluateFallOff);
            }
        }
        return map;
    }

    static float Evaluate(float x, Func<float, float, float, float> eval)
    {
        float a = 3;
        float b = 6f;
        return eval(x, a, b);
    }

    static float EvaluateFallOff(float x, float a, float b)
    {
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + (Mathf.Pow(b - b * x, a)));
    }

    static float EvaluateReverseFallOff(float x, float a, float b)
    {
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) - (Mathf.Pow(b - b * x, a)));
    }
}
