using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCounter : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public GameObject bottomPanel;
    int maxTurns;

    void Awake()
    {
        maxTurns = TCPConnection.instance.serverInfo.numberOfTurns;
    }

    void Update()
    {
        int income = Resources.passiveIncome + Resources.tempIncome;
        text.text = "Turns " + TurnActions.instance.GetCurrentTurn() + "/" + maxTurns;
    }
}
