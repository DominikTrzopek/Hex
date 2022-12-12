using UnityEngine;

public class AttackCommand : ICommand
{
    string ownerId;
    string objectId;
    string enemyId;

    public AttackCommand(CommandBuilder command)
    {
        this.ownerId = command.ownerId;
        this.objectId = command.networkId;
        this.enemyId = command.args[0];
    }

    public void Execute()
    {
        GameObject enemy = FindNetworkObject.FindObj(enemyId);
        if (enemy == null)
            return;

        GameObject playerUnit = FindNetworkObject.FindObj(objectId);
        if (playerUnit == null)
            return;

        playerUnit.GetComponent<TankAttack>().SetEnemy(enemy);

        int attackPoints = playerUnit.GetComponent<UnitStats>().GetAP();
        enemy.GetComponent<StatsAbstract>().AddToReceived(attackPoints);
    }
}
