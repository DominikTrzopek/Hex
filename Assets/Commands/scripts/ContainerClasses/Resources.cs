using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public static int coins = 30;
    public static int passiveIncome = 2;
    public static int tempIncome = 0;
    public List<int> synchronizedCoins;
    public List<int> synchronizedIncome;
    
    public void Start()
    {
        synchronizedCoins = new List<int>(TCPConnection.instance.serverInfo.numberOfPlayers);
        synchronizedIncome = new List<int>(TCPConnection.instance.serverInfo.numberOfPlayers);
    }

    public static void ChangePassiveIncome(int value)
    {
        passiveIncome += value;
    }

    public static void ChangeTmpIncome(int value)
    {
        tempIncome += value;
    }

    public static int GetCoins()
    {
        return coins;
    }

    public static void SetStartingValue(int value)
    {
        coins = value;
    }

    public static void Spend(int value)
    {
        coins -= value;
    }
}
