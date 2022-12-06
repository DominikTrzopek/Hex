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
        if (Input.GetMouseButtonDown(0))
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
        if(command == CommandEnum.INSTANTIANE_UNIT)
            HandleInstantiateUnitCommand();
        else if(command == CommandEnum.MOVE)
            HandleMoveUnitCommand();
    }

    private void checkObjActions()
    {
        if (obj.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
            prepareUi(BaseActions.instance, "BaseActions");
        else if(obj.GetComponent<CustomTag>().HasTag(CellTag.player))
            prepareUi(UnitActions.instance, "UnitActions");
    }

    private void disableUiPlanels()
    {
        foreach(GameObject panel in uiPanels)
        {
            panel.SetActive(false);
        }
    }

    private Collider getRaycast(LayerMask layer)
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayhit, Mathf.Infinity, layer))
            return rayhit.collider;
        return null;
    }

    private void prepareUi(IActionsHandler instance, string panelName)
    {
        instance.setObj(obj);
        disableUiPlanels();
        uiPanels.Find(item => item.name == panelName).SetActive(true);

    }

    private void HandleInstantiateUnitCommand()
    {
        Collider rayhit = getRaycast(layerHex);
        if (rayhit != null)
        {
            GameObject obj = rayhit.transform.gameObject;
            if (obj.GetComponent<CustomTag>().active == true && obj.GetComponent<CustomTag>().taken == false)
            {
                float rotation = HexMetrics.GetRotation(BaseActions.instance.getObjPosition() - obj.transform.position);


                //*******************************************************************************************************
                CommandBuilder builder = new CommandBuilder
                (
                    System.Guid.NewGuid().ToString().Substring(0,18),
                    CommandEnum.INSTANTIANE_UNIT,
                    new List<string>{
                        obj.transform.position.ToString(),
                        rotation.ToString()
                    }
                );

                Debug.Log(builder.saveToString());

                //*******************************************************************************************************

                GameObject newObj = Instantiate(unit, obj.transform.position, Quaternion.Euler(new Vector3(0, rotation, 0)));
                newObj.GetComponent<NetworkId>().position = obj.GetComponent<CustomTag>().coordinates;
                //TODO: poprawić
                newObj.GetComponent<NetworkId>().setIds(UDPServerConfig.getId(), UDPServerConfig.getId());
                //
                foreach (Transform child in newObj.transform)
                {
                    try
                    {
                        child.GetComponent<Renderer>().material.color = Color.blue;
                    }
                    catch { }
                }
                obj.transform.GetChild(1).gameObject.SetActive(false);
                obj.GetComponent<CustomTag>().active = false;
                obj.GetComponent<CustomTag>().taken = true;
                Object.Destroy(obj.transform.GetChild(2).gameObject);

                BaseActions.instance.CancelAction();
                command = CommandEnum.NONE;
            }
        }
    }

    private void HandleMoveUnitCommand()
    {
        Collider rayhit = getRaycast(layerHex);
        if (rayhit != null)
        {
            GameObject end = rayhit.transform.gameObject;
            if (end.GetComponent<CustomTag>().active == true && end.GetComponent<CustomTag>().taken == false)
            {

                //*******************************************************************************************************
                CommandBuilder builder = new CommandBuilder
                (
                    obj.GetComponent<NetworkId>().objectId,
                    CommandEnum.MOVE,
                    new List<string>{
                        end.transform.position.ToString(),
                    }
                );

                Debug.Log(builder.saveToString());

                //*******************************************************************************************************


               
                List<GameObject> path = PathFinding.FindPath(UnitActions.instance.getTakenHex(), end);
                obj.GetComponent<TankMovement>().startSelected = true;
                obj.GetComponent<TankMovement>().setPath(path);
                obj.GetComponent<TankMovement>().enabled = true;
                
                Vector2Int newPosition = path[path.Count - 1].GetComponent<CustomTag>().coordinates;
                path[path.Count - 1].GetComponent<CustomTag>().taken = true;
                path[0].GetComponent<CustomTag>().taken = false;
                obj.GetComponent<NetworkId>().position = newPosition;

                UnitActions.instance.CancelAction();
                command = CommandEnum.NONE;

            }
        }
    }




}
