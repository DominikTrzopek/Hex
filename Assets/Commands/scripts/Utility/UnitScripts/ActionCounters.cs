using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCounters
{
    public static int isMovingCount = 0;
    public static int isAttackingCount = 0;

    public static void Reset()
    {
        isMovingCount = 0;
        isAttackingCount = 0;
    }
}
