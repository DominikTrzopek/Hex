using UnityEngine;

public abstract class ActionsAbstract : MonoBehaviour
{
    protected GameObject obj;
    public IActionHandler handler;
    public TMPro.TextMeshProUGUI textMeshPro;

    public void SetObj(GameObject toSet)
    {
        obj = toSet;
    }

    protected void PerformAction(IActionHandler newHandler)
    {
        if (handler != null)
            handler.CancelAction();
        handler = newHandler;
        handler.MakeAction();
    }

    public void CancelAction()
    {
        if (handler != null)
            handler.CancelAction();
        PlayerActionSelector.command = CommandEnum.NONE;
    }

    protected bool CheckResourceRequirements(int requirement)
    {
        bool check = Resources.GetCoins() >= requirement ? true : false;
        if (!check)
            textMeshPro.text = "Not enough resources!";
        return check;
    }
}
