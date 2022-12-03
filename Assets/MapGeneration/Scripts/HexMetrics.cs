using UnityEngine;

public static class HexMetrics
{

    //****************************************************************************************************************

    public static readonly float outerRadious = 1f;
    public static readonly float innerRadious = 0.25f * Mathf.Sqrt(3);
    const float vector_z = 1.5f;

    //****************************************************************************************************************

    public static readonly Vector3 NW = new Vector3(innerRadious * 2, 0, -vector_z);
    public static readonly Vector3 NE = new Vector3(-innerRadious * 2, 0, -vector_z);
    public static readonly Vector3 W = new Vector3(innerRadious * 4, 0, 0);
    public static readonly Vector3 E = new Vector3(-innerRadious * 4, 0, 0);
    public static readonly Vector3 SW = new Vector3(innerRadious * 2, 0, vector_z);
    public static readonly Vector3 SE = new Vector3(-innerRadious * 2, 0, vector_z);

    //****************************************************************************************************************

    static public float GetRotation(Vector3 positionToCheck)
    {
        if (positionToCheck == W)
            return 0;
        else if (positionToCheck == NW)
            return 60;
        else if (positionToCheck == NE)
            return 120;
        else if (positionToCheck == E)
            return 180;
        else if (positionToCheck == SE)
            return 240;
        else if (positionToCheck == SW)
            return 300;
        return 360;
    }

    //****************************************************************************************************************

}

