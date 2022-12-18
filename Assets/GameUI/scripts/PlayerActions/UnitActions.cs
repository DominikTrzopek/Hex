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
        bool madeMove = obj.GetComponent<Movement>().GetMadeMove();
        bool startSelected = obj.GetComponent<Movement>().GetStartSelected();
        bool inAction = obj.GetComponent<Attack>().GetInAction();
        if (madeMove == true || startSelected == true)
        {
            textMeshPro.text = "This unit has already taken action";
            return;
        }
        else if (inAction == true)
        {
            textMeshPro.text = "Wait for unit to end attacking";
            return;
        }
        PlayerActionSelector.command = CommandEnum.MOVE;
        PerformAction(new MoveHandler(obj));
    }

    public void AttackUnit()
    {
        bool startSelected = obj.GetComponent<Movement>().GetStartSelected();
        bool madeMove = obj.GetComponent<Attack>().GetMadeMove();
        bool inAction = obj.GetComponent<Attack>().GetInAction();
        if (madeMove == true || inAction == true)
        {
            textMeshPro.text = "This unit has already taken action";
            return;
        }
        else if (startSelected == true)
        {
            textMeshPro.text = "Wait for unit to end moving";
            return;
        }

        PlayerActionSelector.command = CommandEnum.ATTACK;
        PerformAction(new AttackHandler(obj, controller));
    }
}
