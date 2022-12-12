using UnityEngine;

public class UnitActions : ActionsAbstract, IActions
{
    public static UnitActions instance { get; private set; }
    public LineController controller;

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
        TankMovement script = obj.GetComponent<TankMovement>();
        if (script.enabled == false || script.moving == true)
        {
            textMeshPro.text = "This unit has already taken action";
            return;
        }

        PlayerActionSelector.command = CommandEnum.MOVE;
        PerformAction(new MoveHandler(obj));
    }

    public void AttackUnit()
    {
        TankAttack script = obj.GetComponent<TankAttack>();
        if (script.enabled == false || script.attacking == true)
        {
            textMeshPro.text = "This unit has already taken action";
            return;
        }
        PlayerActionSelector.command = CommandEnum.ATTACK;
        PerformAction(new AttackHandler(obj, controller));
    }
}
