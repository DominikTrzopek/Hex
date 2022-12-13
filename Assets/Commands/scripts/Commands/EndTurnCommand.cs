using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnCommand : ICommand
{
    GameObject tooltip;
    TMPro.TextMeshProUGUI textMeshPro;
    string playerId;
    int currentTurn;
    Camera camera;

    public EndTurnCommand(CommandBuilder command, Camera camera)
    {
        this.playerId = command.networkId;
        this.currentTurn = int.Parse(command.args[0]);
        this.tooltip = PanelHolder.holder.tooltip;
        this.textMeshPro = PanelHolder.holder.textMeshPro;
        this.camera = camera;
    }

    public void Execute()
    {

        Vector2Int coordinates = FindNetworkObject.FindObj(playerId).GetComponent<NetworkId>().position;
        Vector3 endCamera = new Vector3(coordinates.x * HexMetrics.innerRadious * 4 - 15, 16, coordinates.y * HexMetrics.vector_z);
        camera.orthographicSize = 8;

        camera.GetComponent<Camera_move>().endPosition = endCamera;
        camera.GetComponent<Camera_move>().move = true;
        camera.GetComponent<Camera_move>().doneScrolling = false;

        if(currentTurn > TCPConnection.instance.serverInfo.numberOfTurns)
            Debug.Log("Game ended");
        PlayerScores.container.UpdateScores();
        if (playerId == UDPServerConfig.getId())
        {
            
            PanelHolder.holder.endTurnButton.interactable = true;
            PlayerActionSelector.command = CommandEnum.NONE;
            tooltip.GetComponent<Image>().color = Color.black;
            textMeshPro.text = "Your turn";
            List<GameObject> playerObjects = FindNetworkObject.FindAllNetObj(playerId);
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
            if (obj.GetComponent<TankAttack>())
                obj.GetComponent<TankAttack>().madeMove = false;
            if (obj.GetComponent<TankMovement>())
                obj.GetComponent<TankMovement>().madeMove = false;
        }
    }


}
