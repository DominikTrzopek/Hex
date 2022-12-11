using System.Collections;
using System.Collections.Generic;
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
        playerUnit.GetComponent<TankAttack>().enabled = true;

        // int attackPoints = playerUnit.GetComponent<UnitStats>().getAP();
        // enemy.GetComponent<StatsAbstract>().applyAttackPoints(attackPoints);
    }
}
