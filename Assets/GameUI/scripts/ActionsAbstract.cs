using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionsAbstract : MonoBehaviour
{
    public IActionHandler handler;

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
        SelectPlayerObj.command = CommandEnum.NONE;
    }

    protected bool CheckRequirements(int requirement)
    {
        return Resources.GetCoins() > requirement ? true : false;
    }
}
