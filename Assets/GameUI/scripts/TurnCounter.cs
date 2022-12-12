using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCounter : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public GameObject bottomPanel;
    void Update()
    {
        int income = Resources.passiveIncome + Resources.tempIncome;
        text.text = "Turns " + TurnActions.instance.GetCurrentTurn();
    }
}
