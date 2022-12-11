using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureActions : ActionsAbstract, IPlayerObjectHandler
{
    public static StructureActions instance { get; private set; }
    GameObject obj;

    public GameObject tooltip;
    public TMPro.TextMeshProUGUI textMeshPro;

    public void SetObj(GameObject toSet)
    {
        obj = toSet;
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

    public void InstantiateStructure()
    {
        if (CheckRequirements(Costs.container.initStructure))
        {
            SelectPlayerObj.command = CommandEnum.INSTANTIANE_STRUCTURE;
            PerformAction(new InitStructureHandler(obj));
        }
        else
            textMeshPro.text = "Not enough resources!";
    }

}
