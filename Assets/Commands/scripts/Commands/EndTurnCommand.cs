using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnCommand : ICommand
{
    GameObject bottomPanel = PrefabContainer.container.bottomPanel;
    string playerId;
    int currentTurn;

    public EndTurnCommand(CommandBuilder command)
    {
        this.playerId = command.networkId;
        this.currentTurn = int.Parse(command.args[0]);
    }

    public void Execute()
    {
        TurnActions.instance.SetCurrentTurn(currentTurn);
        if (playerId == UDPServerConfig.getId())
        {
            PlayerActionSelector.command = CommandEnum.NONE;
            bottomPanel.SetActive(true);

            List<GameObject> playerObjects = FindNetworkObject.FindAllNetObj(playerId);
            EnableScripts(playerObjects);
        }
    }

    private void EnableScripts(List<GameObject> playerObjects)
    {
        foreach (GameObject obj in playerObjects)
        {
            if (obj.GetComponent<TankAttack>())
                obj.GetComponent<TankAttack>().madeMove = false;
            if (obj.GetComponent<TankMovement>())
                obj.GetComponent<TankMovement>().madeMove = false;
        }
    }


}
