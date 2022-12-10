using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesUpdate : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    void Update()
    {
        text.text = "Coins:  " + Resources.GetCoins().ToString();
    }
}
