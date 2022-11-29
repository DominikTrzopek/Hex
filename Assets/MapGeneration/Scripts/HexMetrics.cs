using UnityEngine;

public static class HexMetrics
{

    //****************************************************************************************************************

    public const float outerRadious = 1f;
    public const float innerRadious = 0.5f * 0.86602540378f;
    const float vector_z = 1.5f;

    //****************************************************************************************************************

    public static readonly Vector3 NW = new Vector3(innerRadious * 2, 0, -vector_z);
    public static readonly Vector3 NE = new Vector3(-innerRadious * 2, 0, -vector_z);
    public static readonly Vector3 W = new Vector3(innerRadious * 4, 0, 0); 
    public static readonly Vector3 E = new Vector3(-innerRadious * 4, 0, 0); 
    public static readonly Vector3 SW = new Vector3(innerRadious * 2, 0, vector_z);
    public static readonly Vector3 SE = new Vector3(-innerRadious * 2, 0, vector_z);

    //****************************************************************************************************************

    static public float GetRotation(Vector3 position_to_check)
    {
        if (position_to_check == W)
            return 0;
        else if (position_to_check == NW)
            return 60;
        else if (position_to_check == NE)
            return 120;
        else if (position_to_check == E)
            return 180;
        else if (position_to_check == SE)
            return 240;
        else if (position_to_check == SW)
            return 300;
        return 360;
    }

    //****************************************************************************************************************

}

