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
        if(PlayerActionSelector.command == CommandEnum.ENDTURN)
            return;
        List<GameObject> panels = PanelHolder.holder.panels;
        if (PlayerActionSelector.command != CommandEnum.NONE)
        {
            Debug.Log("End or cancel your current action");
            textMeshPro.text = "End or cancel your current action";
            return;
        }
        if(ActionCounters.isAttackingCount != 0 || ActionCounters.isMovingCount != 0)
        {
            Debug.Log("wait for units");
            textMeshPro.text = "Wait for units to stop";
            return;
        }
        foreach (IActions actions in PanelHolder.holder.bottomPanel.GetComponents<IActions>())
        {
            actions.CancelAction();
        }
        currentTurn++;
        PanelHolder.holder.DisableUiPlanels();
        PlayerActionSelector.command = CommandEnum.ENDTURN;
        CommandBuilder builder = new CommandBuilder
        (
            "##", //set by server
            CommandEnum.ENDTURN,
            new List<string>{
                currentTurn.ToString(),
                Resources.coins.ToString(),
            },
            new GameState()
        );
        Debug.Log(builder.SaveToString());

        TCPConnection.instance.client.writeSocket(builder);
    }
}
