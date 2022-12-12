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
        TankMovement movement = obj.GetComponent<TankMovement>();
        TankAttack attack = obj.GetComponent<TankAttack>();
        if (movement.madeMove == true || movement.startSelected == true)
        {
            textMeshPro.text = "This unit has already taken action";
            return;
        }
        else if (attack.inAction == true)
        {
            textMeshPro.text = "Wait for unit to end attacking";
            return;
        }
        PlayerActionSelector.command = CommandEnum.MOVE;
        PerformAction(new MoveHandler(obj));
    }

    public void AttackUnit()
    {
        TankMovement movement = obj.GetComponent<TankMovement>();
        TankAttack attack = obj.GetComponent<TankAttack>();
        if (attack.madeMove == true || attack.inAction == true)
        {
            textMeshPro.text = "This unit has already taken action";
            return;
        }
        else if (movement.startSelected == true)
        {
            textMeshPro.text = "Wait for unit to end moving";
            return;
        }

        PlayerActionSelector.command = CommandEnum.ATTACK;
        PerformAction(new AttackHandler(obj, controller));
    }
}
