using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionSelector : MonoBehaviour
{
    private List<GameObject> uiPanels;
    public static CommandEnum command = CommandEnum.NONE;
    private GameObject obj;

    LayerMask layerPlayer, layerHex;
    private void Awake()
    {
        command = CommandEnum.ENDTURN;
        layerPlayer = LayerMask.GetMask("Player");
        layerHex = LayerMask.GetMask("Default");
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) || ((int)command >= 3 && (int)command <= 8))
        {
            Collider rayhit = GetRaycast(layerPlayer);
            if (rayhit != null && command == CommandEnum.NONE)
            {
                obj = rayhit.transform.parent.gameObject;

                if (obj.GetComponent<NetworkId>().ownerId == UDPServerConfig.GetId())
                {
                    CheckObjActions();
                }
            }
            EnableActionHandler();
        }
    }

    private void EnableActionHandler()
    {
        if (command == CommandEnum.INSTANTIANE_UNIT)
            HandleInstantiateUnitCommand();
        else if (command == CommandEnum.MOVE)
            HandleMoveUnitCommand();
        else if (command == CommandEnum.MAKE_BANK)
            HandleMakeBankCommand();
        else if (command == CommandEnum.INSTANTIANE_STRUCTURE)
            HandleInstantiateStructureCommand();
        else if (command == CommandEnum.ATTACK)
            HandleAttackCommand();
        else if ((int)command >= 3 && (int)command <= 8)
            HandleUpgradeCommand(command);
    }

    private void CheckObjActions()
    {
        if (obj.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
            PrepareUi(BaseActions.instance, "BaseActions");
        else if (obj.GetComponent<CustomTag>().HasTag(CellTag.player))
            PrepareUi(UnitActions.instance, "UnitActions");
        else if (obj.GetComponent<CustomTag>().HasTag(CellTag.building))
            PrepareUi(StructureActions.instance, "StructureActions");
    }

    private Collider GetRaycast(LayerMask layer)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayhit, Mathf.Infinity, layer))
            return rayhit.collider;
        return null;
    }

    private void PrepareUi(IActions instance, string panelName)
    {
        instance.SetObj(obj);
        PanelHolder.holder.DisableUiPlanels();
        
        uiPanels = PanelHolder.holder.panels;
        uiPanels.Find(item => item.name == panelName).SetActive(true);

    }

    private void HandleInstantiateUnitCommand()
    {
        Collider rayhit = GetRaycast(layerHex);
        if (rayhit != null)
        {
            GameObject obj = rayhit.transform.gameObject;
            if (obj.GetComponent<CustomTag>().active == true && obj.GetComponent<CustomTag>().taken == false)
            {
                float rotation = HexMetrics.GetRotation(BaseActions.instance.GetObjPosition() - obj.transform.position);
                Vector2Int position = obj.GetComponent<CustomTag>().coordinates;
                int unitPrefabNumber = 0;

                CommandBuilder builder = new CommandBuilder
                (
                    System.Guid.NewGuid().ToString().Substring(0, 12),
                    CommandEnum.INSTANTIANE_UNIT,
                    new List<string>{
                        position.x.ToString(),
                        position.y.ToString(),
                        rotation.ToString(),
                        unitPrefabNumber.ToString()
                    },
                    new GameState(false)
                );
                Resources.Spend(Costs.container.initUnit);

                TCPConnection.instance.client.WriteSocket(builder);
                BaseActions.instance.CancelAction();
            }
        }
    }

    private void HandleMakeBankCommand()
    {
        CommandBuilder builder = new CommandBuilder
        (
            obj.GetComponent<NetworkId>().objectId,
            CommandEnum.MAKE_BANK,
            null,
            new GameState(false)
        );
        Resources.Spend(Costs.container.makeBank);
        Resources.ChangePassiveIncome(1);

        TCPConnection.instance.client.WriteSocket(builder);
        BaseActions.instance.CancelAction();
    }

    private void HandleInstantiateStructureCommand()
    {
        Collider rayhit = GetRaycast(layerHex);
        if (rayhit != null)
        {
            GameObject end = rayhit.transform.gameObject;
            if (end.GetComponent<CustomTag>().active == true && end.GetComponent<CustomTag>().taken == false)
            {
                List<GameObject> path = PathFinding.FindPath(end);
                List<string> cellsInPath = new List<string>();
                cellsInPath.Add(obj.GetComponent<NetworkId>().objectId);
                for (int i = 0; i < path.Count; i++)
                {
                    cellsInPath.Add(path[i].GetComponent<CustomTag>().coordinates.ToString());
                }
                CommandBuilder builder = new CommandBuilder
                (
                    System.Guid.NewGuid().ToString().Substring(0, 12),
                    CommandEnum.INSTANTIANE_STRUCTURE,
                    cellsInPath,
                    new GameState(false)
                );
                Resources.Spend(Costs.container.initStructure);
                Resources.ChangePassiveIncome(2);

                TCPConnection.instance.client.WriteSocket(builder);
                if (obj.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
                    BaseActions.instance.CancelAction();
                else
                    StructureActions.instance.CancelAction();
            }
        }
    }

    private void HandleMoveUnitCommand()
    {
        Collider rayhit = GetRaycast(layerHex);
        if (rayhit != null)
        {
            GameObject end = rayhit.transform.gameObject;
            if (end.GetComponent<CustomTag>().active == true && end.GetComponent<CustomTag>().taken == false)
            {
                List<GameObject> path = PathFinding.FindPath(end);
                List<string> cellsInPath = new List<string>();
                for (int i = 0; i < path.Count; i++)
                {
                    cellsInPath.Add(path[i].GetComponent<CustomTag>().coordinates.ToString());
                }
                CommandBuilder builder = new CommandBuilder
                (
                    obj.GetComponent<NetworkId>().objectId,
                    CommandEnum.MOVE,
                    cellsInPath,
                    new GameState(false)
                );
                TCPConnection.instance.client.WriteSocket(builder);
                UnitActions.instance.CancelAction();

            }
        }
    }

    private void HandleAttackCommand()
    {
        Collider rayhit = GetRaycast(layerPlayer);
        if (rayhit != null)
        {
            GameObject end = rayhit.transform.gameObject;
            Vector2Int endPosition = end.transform.parent.GetComponent<NetworkId>().position;
            GameObject cell = HexGrid.hexArray[endPosition.x, endPosition.y];
            if (cell.GetComponent<CustomTag>().active == true)
            {
                CommandBuilder builder = new CommandBuilder
                (
                    obj.GetComponent<NetworkId>().objectId,
                    CommandEnum.ATTACK,
                    new List<string>{
                        end.transform.parent.GetComponent<NetworkId>().objectId
                    },
                    new GameState(false)
                );
                Debug.Log(builder.SaveToString());

                TCPConnection.instance.client.WriteSocket(builder);
                UnitActions.instance.CancelAction();
            }
        }
    }

    private void HandleUpgradeCommand(CommandEnum upgrade)
    {
        CommandBuilder builder = new CommandBuilder
        (
            obj.GetComponent<NetworkId>().objectId,
            upgrade,
            null,
            new GameState(false)
        );
        switch (upgrade)
        {
            case CommandEnum.UPGRADE_CHASIS:
                Resources.Spend(Costs.container.upgradeChasis);
                break;
            case CommandEnum.UPGRADE_ENGINE:
                Resources.Spend(Costs.container.upgradeEngine);
                break;
            case CommandEnum.UPGRADE_GUN:
                Resources.Spend(Costs.container.upgradeGun);
                break;
            case CommandEnum.UPGRADE_RADIO:
                Resources.Spend(Costs.container.upgradeRadio);
                break;
            case CommandEnum.UPGRADE_STRUCTURE:
                Resources.Spend(Costs.container.upgradeStructure);
                break;
        }

        TCPConnection.instance.client.WriteSocket(builder);
        BaseActions.instance.CancelAction();
    }


}
