using UnityEngine;
using System;

public class CommandExecutionHandler : MonoBehaviour
{
    public Camera cam;
    public LevelLoader loader;
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
                CommandBuilder builder = CommandBuilder.FromString(conn.messageQueue[0]);
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
                    case CommandEnum.ENDTURN:
                        invoker.ExecuteCommand(new EndTurnCommand(builder, cam));
                        break;
                    case CommandEnum.ENDGAME:
                        invoker.ExecuteCommand(new EndGameCommand(loader));
                        break;
                }
            }
            catch (ArgumentException err)
            {
                Debug.Log(conn.messageQueue[0]);
                Debug.Log(err);
            }
            conn.messageQueue.RemoveAt(0);
        }
    }
}
