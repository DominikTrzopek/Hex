using UnityEngine;

public class StructureActions : ActionsAbstract, IActions
{
    public static StructureActions instance { get; private set; }

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

    public void InstantiateStructure()
    {
        if (CheckResourceRequirements(Costs.container.initStructure))
        {
            PlayerActionSelector.command = CommandEnum.INSTANTIANE_STRUCTURE;
            PerformAction(new InitStructureHandler(obj));
        }
    }

}
