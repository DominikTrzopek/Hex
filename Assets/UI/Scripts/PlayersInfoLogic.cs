using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersInfoLogic : MonoBehaviour
{

    public Image image;
    public TMPro.TMP_Text readyText;
    public TMPro.TMP_Text nameText;
    public PlayerInfo playerInfo;

    private static int iterator;

    public void OnEnable(){
        iterator = 0;
        image.color = ColorList.colors[0];
        ColorList.colors.RemoveAt(0);
        playerInfo.id = null;
    }

    public void OnDisable()
    {
        ColorList.colors.Add(image.color);
    }

    void Update()
    {
        nameText.text = playerInfo.name;
        FormatStatus();
    }

    public void SetColorForward(){
        iterator++;
        if(iterator >= ColorList.colors.Count){
            iterator = 0;
        }
        Color old = image.color;
        image.color = ColorList.colors[iterator];
        ColorList.colors.RemoveAt(iterator);
        ColorList.colors.Add(old);
    }

        public void SetColorBackward(){
        iterator--;
        if(iterator < 0){
            iterator = ColorList.colors.Count - 1;
        }
        Color old = image.color;
        image.color = ColorList.colors[iterator];
        ColorList.colors.RemoveAt(iterator);
        ColorList.colors.Add(old);
    }

    private void FormatStatus()
    {
        switch(playerInfo.status)
        {
            case PlayerStatus.NOTREADY:
                readyText.text = "NOT READY";
                readyText.color = Color.red;
                break;
            case PlayerStatus.READY:
                readyText.text = "READY";
                readyText.color = Color.green;
                break;
            default:
                readyText.text = "NOT CONNECTED";
                readyText.color = Color.gray;
                break;
        }
    }
}
