using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerObj : MonoBehaviour
{
    public List<GameObject> uiPanels;
    public GameObject unit;
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
        if (Input.GetMouseButtonDown(0) || command == CommandEnum.MAKE_BANK)
        {
            Collider rayhit = getRaycast(layerPlayer);
            if (rayhit != null && command == CommandEnum.NONE)
            {
                obj = rayhit.transform.parent.gameObject;

                if (obj.GetComponent<NetworkId>().ownerId == UDPServerConfig.getId())
                {
                    checkObjActions();
                }
            }
            enableActionHandler();
        }
    }

    private void enableActionHandler()
    {
        if (command == CommandEnum.INSTANTIANE_UNIT)
            HandleInstantiateUnitCommand();
        else if (command == CommandEnum.MOVE)
            HandleMoveUnitCommand();
        else if (command == CommandEnum.MAKE_BANK)
            HandleMakeBankCommand();
        else if (command == CommandEnum.INSTANTIANE_STRUCTURE)
            HandleInstantiateStructureCommand();
    }

    private void checkObjActions()
    {
        if (obj.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
            prepareUi(BaseActions.instance, "BaseActions");
        else if (obj.GetComponent<CustomTag>().HasTag(CellTag.player))
            prepareUi(UnitActions.instance, "UnitActions");
    }

    private void disableUiPlanels()
    {
        foreach (GameObject panel in uiPanels)
        {
            panel.SetActive(false);
        }
    }

    private Collider getRaycast(LayerMask layer)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayhit, Mathf.Infinity, layer))
            return rayhit.collider;
        return null;
    }

    private void prepareUi(IPlayerObjectHandler instance, string panelName)
    {
        instance.SetObj(obj);
        disableUiPlanels();
        uiPanels.Find(item => item.name == panelName).SetActive(true);

    }

    //*******************************************************************************************************************

    private void HandleInstantiateUnitCommand()
    {
        Collider rayhit = getRaycast(layerHex);
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
        Collider rayhit = getRaycast(layerHex);
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
                BaseActions.instance.CancelAction();
            }
        }
    }

    //****************************************************************************************************
    
    private void HandleMoveUnitCommand()
    {
        Collider rayhit = getRaycast(layerHex);
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


}
