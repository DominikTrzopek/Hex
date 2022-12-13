using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public TMPro.TextMeshProUGUI name1;
    public TMPro.TextMeshProUGUI name2;
    public TMPro.TextMeshProUGUI name3;

    public TMPro.TextMeshProUGUI score1;
    public TMPro.TextMeshProUGUI score2;
    public TMPro.TextMeshProUGUI score3;


    void OnEnable()
    {
        List<PlayerScores.ScoreData> scores;
        scores = PlayerScores.container.GetScores();

        SetText(scores, name1, score1);
        SetText(scores, name2, score2);
        SetText(scores, name3, score3);
    }

    PlayerScores.ScoreData FindMax(List<PlayerScores.ScoreData> scores)
    {
        int max = 0;
        string name;
        int maxIndex = 0;
        for (int i = 0; i < scores.Count; i++)
        {
            if (scores[i].value > max)
            {
                max = scores[i].value;
                name = scores[i].name;
                maxIndex = i;
            }
        }
        return scores[maxIndex];
    }

    void SetText(List<PlayerScores.ScoreData> scores, TMPro.TextMeshProUGUI name, TMPro.TextMeshProUGUI value)
    {
        try
        {
            PlayerScores.ScoreData player = FindMax(scores);
            scores.Remove(player);
            name.text = player.name;
            value.text = player.value.ToString();
        }
        catch
        {
            name.text = "";
            value.text = "";
        }
    }

}
