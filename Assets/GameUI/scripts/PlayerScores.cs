using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScores : MonoBehaviour
{
    private List<ScoreData> scores;
    public GameObject scorePrefab;
    public static PlayerScores container;

    private void Awake()
    {
        if (container != null && container != this)
        {
            Destroy(this);
        }
        else
        {
            container = this;
        }
    }

    private void Start()
    {
        scores = new List<ScoreData>();
        TCPConnection conn = TCPConnection.instance;
        foreach (PlayerInfo player in conn.playerInfo)
        {
            GameObject newCell = Instantiate(scorePrefab);
            newCell.transform.SetParent(this.gameObject.transform, false);

            Image img = newCell.transform.Find("panel").GetComponent<Image>();
            Color color = player.color;
            color.a = 0.15f;
            img.color = color;

            TMPro.TextMeshProUGUI name = newCell.transform.Find("name").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI score = newCell.transform.Find("value").GetComponent<TMPro.TextMeshProUGUI>();
            scores.Add(new ScoreData(player.id, player.name, 0, score));

            name.text = player.name;
            score.text = "0";
        }
    }

    public List<ScoreData> GetScores()
    {
        return scores;
    }


    public void UpdateScores()
    {
        TCPConnection conn = TCPConnection.instance;
        for (int i = 0; i < scores.Count; i++)
        {
            scores[i].value = 0;
            foreach (GameObject hex in HexGrid.hexArray)
            {
                CustomTag tag = hex.GetComponent<CustomTag>();
                if (scores[i].id == tag.takenBy)
                {
                    scores[i].value++;
                }
            }
            scores[i].text.text = scores[i].value.ToString();
        }
    }

    public class ScoreData
    {
        public string id;
        public string name;
        public int value;
        public TMPro.TextMeshProUGUI text;

        public ScoreData(string id, string name, int value, TMPro.TextMeshProUGUI text)
        {
            this.id = id;
            this.name = name;
            this.value = value;
            this.text = text;
        }
    }
}
