using UnityEngine;

public class ResourcesUpdate : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    void Update()
    {
        int income = Resources.passiveIncome + Resources.tempIncome;
        text.text = "Coins:  " + Resources.GetCoins().ToString() + " (+" + income + ")" + " " + ActionCounters.isMovingCount;
    }
}
