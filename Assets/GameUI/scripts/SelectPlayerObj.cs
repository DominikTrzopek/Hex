using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerObj : MonoBehaviour
{
    public List<GameObject> uiPanels;
    public static CommandEnum command = CommandEnum.NONE;
    private GameObject obj;

    LayerMask layerPlayer, layerHex;
    private void Start()
    {
        layerPlayer = LayerMask.GetMask("Player");
        layerHex = LayerMask.GetMask("Default");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || ((int)command >= 2  && (int)command <= 7))
        {
            Collider rayhit = GetRaycast(layerPlayer);
            if (rayhit != null && command == CommandEnum.NONE)
            {
                obj = rayhit.transform.parent.gameObject;

                if (obj.GetComponent<NetworkId>().ownerId == UDPServerConfig.getId())
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
        else if ((int)command >= 2  && (int)command <= 6)
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

    private void DisableUiPlanels()
    {
        foreach (GameObject panel in uiPanels)
        {
            panel.SetActive(false);
        }
    }

    private Collider GetRaycast(LayerMask layer)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayhit, Mathf.Infinity, layer))
            return rayhit.collider;
        return null;
    }

    private void PrepareUi(IPlayerObjectHandler instance, string panelName)
    {
        instance.SetObj(obj);
        DisableUiPlanels();
        uiPanels.Find(item => item.name == panelName).SetActive(true);

    }

    //*******************************************************************************************************************

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

                CommandBuilder builder = new CommandBuilder
                (
                    System.Guid.NewGuid().ToString().Substring(0, 18),
                    CommandEnum.INSTANTIANE_UNIT,
                    new List<string>{
                        position.x.ToString(),
                        position.y.ToString(),
                        rotation.ToString()
                    }
                );
                //************************************

                TCPConnection.instance.messageQueue.Add(builder.saveToString());

                //wysłanie danych na serwer

                //************************************
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
            null
        );
        //************************************

        TCPConnection.instance.messageQueue.Add(builder.saveToString());
        //wysłanie danych na serwer
        //*************************************
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
                    System.Guid.NewGuid().ToString().Substring(0, 18),
                    CommandEnum.INSTANTIANE_STRUCTURE,
                    cellsInPath
                );
                //************************************

                TCPConnection.instance.messageQueue.Add(builder.saveToString());
                Debug.Log(builder.saveToString());
                //wysłanie danych na serwer
                //*************************************
                if(obj.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
                    BaseActions.instance.CancelAction();
                else
                    StructureActions.instance.CancelAction();
            }
        }
    }

    //****************************************************************************************************
    
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
                    cellsInPath
                );
                //************************************

                TCPConnection.instance.messageQueue.Add(builder.saveToString());

                //wysłanie danych na serwer
                //*************************************
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
            null
        );
        //************************************

        TCPConnection.instance.messageQueue.Add(builder.saveToString());
        Debug.Log(builder.saveToString());
        //wysłanie danych na serwer
        //*************************************
        BaseActions.instance.CancelAction();
    }


}
