using UnityEngine;

public class BaseActions : ActionsAbstract, IActions
{
    public static BaseActions instance { get; private set; }
    public GameObject uiImage;

    public Vector3 GetObjPosition()
    {
        return obj.transform.position;
    }

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

    public void InstantiateUnit()
    {
        if (CheckResourceRequirements(Costs.container.initUnit))
        {
            PlayerActionSelector.command = CommandEnum.INSTANTIANE_UNIT;
            PerformAction(new InitUnitHandler(obj, uiImage));
        }
    }

    public void MakeBank()
    {
        if (CheckResourceRequirements(Costs.container.makeBank))
        {
            PlayerActionSelector.command = CommandEnum.MAKE_BANK;
            PerformAction(new MakeBankHandler(obj));
        }
    }

    public void InstantiateStructure()
    {
        if (CheckResourceRequirements(Costs.container.initStructure))
        {
            PlayerActionSelector.command = CommandEnum.INSTANTIANE_STRUCTURE;
            PerformAction(new InitStructureHandler(obj));
        }
    }

}
