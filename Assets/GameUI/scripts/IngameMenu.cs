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
        ActionCounters.isAttackingCount = 0;
        ActionCounters.isMovingCount = 0;
        TCPConnection.instance.clear();
    }

    void Update()
    {
        if (PlayerActionSelector.command != CommandEnum.NONE)
        {
            Debug.Log("dfgdfgfdg");
            PanelHolder.holder.endTurnButton.interactable = false;
        }
        else if(ActionCounters.isAttackingCount != 0 || ActionCounters.isMovingCount != 0)
        {
            Debug.Log(ActionCounters.isMovingCount);
            Debug.Log(ActionCounters.isAttackingCount);
            PanelHolder.holder.endTurnButton.interactable = false;
        }
        else if(PlayerActionSelector.command != CommandEnum.ENDTURN)
        {
            PanelHolder.holder.endTurnButton.interactable = true;
        }

    }

}
