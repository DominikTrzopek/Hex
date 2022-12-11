using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActions : ActionsAbstract, IPlayerObjectHandler
{
    public static UnitActions instance { get; private set; }
    public LineController controller;
    GameObject obj;
    private List<GameObject> objInRange;

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

    public void MoveUnit()
    {
        if (obj.GetComponent<TankMovement>().moving == true)
        {
            textMeshPro.text = "Unit is moving!";
            return;
        }

        SelectPlayerObj.command = CommandEnum.MOVE;
        PerformAction(new MoveHandler(obj));
    }

    public void AttackUnit()
    {
        if (obj.GetComponent<TankAttack>().enabled == true)
        {
            textMeshPro.text = "Unit is attacking!";
            return;
        }
        SelectPlayerObj.command = CommandEnum.ATTACK;
        PerformAction(new AttackHandler(obj, controller));
    }
}
