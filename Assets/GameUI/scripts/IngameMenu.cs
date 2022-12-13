using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour
{
    public LevelLoader levelLoader;

    public void Leave()
    {
        SceneManager.LoadScene(0);
        Resources.Clear();
        PlayerActionSelector.command = CommandEnum.NONE;
        TankAttack.isAttackingCount = 0;
        TankMovement.isMovingCount = 0;
        TCPConnection.instance.clear();
    }

    void Update()
    {
        if (PlayerActionSelector.command != CommandEnum.NONE)
        {
            PanelHolder.holder.endTurnButton.interactable = false;
        }
        else if(TankAttack.isAttackingCount != 0 || TankMovement.isMovingCount != 0)
        {
            PanelHolder.holder.endTurnButton.interactable = false;
        }
        else if(PlayerActionSelector.command != CommandEnum.ENDTURN)
        {
            PanelHolder.holder.endTurnButton.interactable = true;
        }

    }

}
