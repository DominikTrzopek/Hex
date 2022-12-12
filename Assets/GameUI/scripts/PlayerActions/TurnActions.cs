using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnActions : ActionsAbstract, IActions
{
    public static TurnActions instance { get; private set; }
    private int currentTurn = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }
    public void SetCurrentTurn(int value)
    {
        currentTurn = value;
    }

    public void NextTurn()
    {
        GameObject bottomPanel = PrefabContainer.container.bottomPanel;
        if (PlayerActionSelector.command != CommandEnum.NONE)
        {
            textMeshPro.text = "End or cancel your current action";
            return;
        }
        foreach (IActions actions in bottomPanel.GetComponents<IActions>())
        {
            actions.CancelAction();
        }
        bottomPanel.SetActive(false);
        PlayerActionSelector.command = CommandEnum.ENDTURN;
        currentTurn++;
        CommandBuilder builder = new CommandBuilder
        (
            UDPServerConfig.getId(), //set by server
            CommandEnum.ENDTURN,
            new List<string>{
                currentTurn.ToString()
            }
        );

        //************************************

        TCPConnection.instance.messageQueue.Add(builder.SaveToString());

        //wysłanie danych na serwer
        //*************************************

    }
}
