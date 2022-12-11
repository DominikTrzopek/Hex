using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScores : MonoBehaviour
{
    public List<TMPro.TextMeshProUGUI> scores;
    public GameObject scorePrefab;

    void Awake()
    {
        // TCPConnection conn = TCPConnection.instance;
        // foreach (PlayerInfo player in conn.playerInfo)
        // {
        //     GameObject newCell = Instantiate(scorePrefab);
        //     scorePrefab.transform.SetParent(this.gameObject.transform, false);

        //     TMPro.TextMeshProUGUI name = newCell.transform.Find("name").GetComponent<TMPro.TextMeshProUGUI>();
        //     scores.Add(newCell.transform.Find("value").GetComponent<TMPro.TextMeshProUGUI>());

        //     name.text = player.name;
        // }
    }

}
