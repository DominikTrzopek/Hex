using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EndTurnCommand : ICommand
{
    GameObject tooltip;
    TMPro.TextMeshProUGUI textMeshPro;
    string playerId;
    int currentTurn;
    int resources;
    GameState gameState;
    Camera camera;
    bool addCoins;

    public EndTurnCommand(CommandBuilder command, Camera camera)
    {
        this.playerId = command.networkId;
        if (command.args.Count == 1)
            this.currentTurn = int.Parse(command.args[0]);
        if (command.args.Count == 2)
            this.resources = int.Parse(command.args[1]);
        this.gameState = command.gameState;
        this.tooltip = PanelHolder.holder.tooltip;
        this.textMeshPro = PanelHolder.holder.textMeshPro;
        this.camera = camera;
        this.addCoins = true;
    }

    public void Execute()
    {

        Vector2Int coordinates = FindNetworkObject.FindObj(playerId).GetComponent<NetworkId>().position;
        Vector3 endCamera = new Vector3(coordinates.x * HexMetrics.innerRadious * 4 - 15, 16, coordinates.y * HexMetrics.vector_z);
        camera.orthographicSize = 8;

        camera.GetComponent<Camera_move>().endPosition = endCamera;
        camera.GetComponent<Camera_move>().move = true;
        camera.GetComponent<Camera_move>().doneScrolling = false;

        if (ConnectionHandler.reconnected)
        {
            try
            {
                addCoins = false;
                ConnectionHandler.reconnected = false;
                ConnectionHandler.Reinstantiate(gameState);
            }
            catch(NullReferenceException){}
        }

        PlayerScores.container.UpdateScores();
        if (playerId == UDPServerConfig.getId())
        {
            PanelHolder.holder.endTurnButton.interactable = true;
            PlayerActionSelector.command = CommandEnum.NONE;
            tooltip.GetComponent<Image>().color = Color.black;
            textMeshPro.text = "Your turn";
            List<GameObject> playerObjects = FindNetworkObject.FindPlayerAllNetObj(playerId);
            if(addCoins)
                Resources.coins += Resources.passiveIncome + Resources.tempIncome;
            EnableScripts(playerObjects);
        }
        else
        {
            PanelHolder.holder.endTurnButton.interactable = false;
            tooltip.SetActive(true);
            tooltip.GetComponent<Image>().color = PlayerInfoGetter.GetColor(playerId);
            textMeshPro.text = PlayerInfoGetter.GetName(playerId) + " turn";
        }
    }

    private void EnableScripts(List<GameObject> playerObjects)
    {
        foreach (GameObject obj in playerObjects)
        {
            try
            {
                obj.GetComponent<Attack>().SetMadeMove(false);
                obj.GetComponent<Movement>().SetMadeMove(false);
            }
            catch { }
        }
    }


}
