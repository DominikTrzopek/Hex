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

    private List<Color> colors = new List<Color>{
        Color.blue,
        Color.green,
        Color.magenta,
        Color.red,
        Color.yellow,
        Color.cyan
    };

    private static int iterator;
    private static bool ready;

    public void Awake(){
        iterator = 0;
        ready = false;
        image.color = Color.gray;
        playerInfo.id = null;
    }

    void Update()
    {
        Debug.Log(playerInfo.id);
        nameText.text = playerInfo.name;
        FormatStatus();
    }

    public void SetColorForward(){
        iterator++;
        if(iterator >= colors.Count){
            iterator = 0;
        }
        image.color = colors[iterator];
    }

        public void SetColorBackward(){
        iterator--;
        if(iterator < 0){
            iterator = colors.Count - 1;
        }
        image.color = colors[iterator];
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

    public void SetReadyStatus(){
        if(ready == false){
            ready = true;
            readyText.SetText("Ready");
            readyText.color = Color.green;
        }
        else{
            ready = false;
            readyText.SetText("Not Ready");
            readyText.color = Color.red;
        }

        //TODO: send message to TCP server with player info

    }
}
