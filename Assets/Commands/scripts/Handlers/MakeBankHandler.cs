using UnityEngine;

public class MakeBankHandler : IActionHandler
{
    GameObject obj;

    public MakeBankHandler(GameObject obj)
    {
        this.obj = obj;
    }

    public void MakeAction()
    {

    }

    public void CancelAction()
    {

    }

    ~MakeBankHandler()
    {
        CancelAction();
    }
}
