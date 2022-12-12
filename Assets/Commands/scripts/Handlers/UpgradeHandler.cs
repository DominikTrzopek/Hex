using UnityEngine;

public class UpgradeHandler : IActionHandler
{
    GameObject obj;
    CommandEnum upgrade;

    public UpgradeHandler(GameObject obj, CommandEnum upgrade)
    {
        this.obj = obj;
        this.upgrade = upgrade;
    }

    public void MakeAction()
    {

    }

    public void CancelAction()
    {

    }

    ~UpgradeHandler()
    {
        CancelAction();
    }
}
