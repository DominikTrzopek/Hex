using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandExecutionHandler : MonoBehaviour
{

    TCPConnection conn;
    Invoker invoker = new Invoker();

    void Start()
    {
        conn = TCPConnection.instance;
    }

    void Update()
    {
        if (conn.messageQueue.Count > 0)
        {
            try
            {
                CommandBuilder builder = CommandBuilder.fromString(conn.messageQueue[0]);
                switch (builder.command)
                {
                    case CommandEnum.INSTANTIANE_UNIT:
                        invoker.ExecuteCommand(new InitUnitCommand(builder));
                        break;
                    case CommandEnum.MOVE:
                        invoker.ExecuteCommand(new MoveCommand(builder));
                        break;
                    case CommandEnum.MAKE_BANK:
                        invoker.ExecuteCommand(new MakeBankCommand(builder));
                        break;
                    case CommandEnum.INSTANTIANE_STRUCTURE:
                        invoker.ExecuteCommand(new InitStructureCommand(builder));
                        break;
                    case CommandEnum.ATTACK:
                        invoker.ExecuteCommand(new AttackCommand(builder));
                        break;
                    case CommandEnum.UPGRADE_GUN:
                        invoker.ExecuteCommand(new UpgradeGunCommand(builder));
                        break;
                    case CommandEnum.UPGRADE_STRUCTURE:
                        invoker.ExecuteCommand(new UpgradeHPCommand(builder));
                        break;
                    case CommandEnum.UPGRADE_CHASIS:
                        invoker.ExecuteCommand(new UpgradeHPCommand(builder));
                        break;
                    case CommandEnum.UPGRADE_ENGINE:
                        invoker.ExecuteCommand(new UpgradeEngineCommand(builder));
                        break;
                    case CommandEnum.UPGRADE_RADIO:
                        invoker.ExecuteCommand(new UpgradeRadioCommand(builder));
                        break;
                }
            }
            catch (ArgumentException err)
            {
                Debug.Log(err);
            }
            conn.messageQueue.RemoveAt(0);
        }
    }
}
