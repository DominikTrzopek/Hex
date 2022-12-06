using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerObj : MonoBehaviour
{
    public GameObject uiImage;
    public GameObject unit;
    public GameObject mainUiPanel;
    public GameObject baseUiPanel;
    public GameObject unitUiPanel;
    private Vector3 basePosition;
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayhitStart, Mathf.Infinity, layerPlayer) && command == CommandEnum.NONE)
            {
                obj = rayhitStart.collider.transform.parent.gameObject;

                if (obj.GetComponent<NetworkId>().ownerId == UDPServerConfig.getId())
                {
                    Debug.Log(obj.name);
                    if (obj.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
                    {

                        BaseActions.instance.obj = obj;
                        BaseActions.instance.uiImage = uiImage;
                        unitUiPanel.SetActive(false);
                        baseUiPanel.SetActive(true);
                        basePosition = obj.transform.position;
                    }
                    else if(obj.GetComponent<CustomTag>().HasTag(CellTag.player))
                    {
                        UnitActions.instance.obj = obj;
                        baseUiPanel.SetActive(false);
                        unitUiPanel.SetActive(true);
                    }
                }
            }

            if(command == CommandEnum.INSTANTIANE_UNIT)
                HandleInstantiateUnitCommand();
            else if(command == CommandEnum.MOVE)
                HandleMoveUnitCommand();
            

        }
    }


    private void HandleInstantiateUnitCommand()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayhitEnd, Mathf.Infinity, layerHex))
        {
            GameObject obj = rayhitEnd.collider.transform.gameObject;
            if (obj.GetComponent<CustomTag>().active == true && obj.GetComponent<CustomTag>().taken == false)
            {
                float rotation = HexMetrics.GetRotation(basePosition - obj.transform.position);
                GameObject newObj = Instantiate(unit, obj.transform.position, Quaternion.Euler(new Vector3(0, rotation, 0)));
                //newObj.GetComponent<NetworkId>().setIds()
                newObj.GetComponent<NetworkId>().position = obj.GetComponent<CustomTag>().coordinates;
                newObj.GetComponent<NetworkId>().setIds(UDPServerConfig.getId(), UDPServerConfig.getId());
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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayhitEnd, Mathf.Infinity, layerHex))
        {
            GameObject end = rayhitEnd.collider.transform.gameObject;
            if (end.GetComponent<CustomTag>().active == true && end.GetComponent<CustomTag>().taken == false)
            {
                
                List<GameObject> path = PathFinding.FindPath(UnitActions.instance.takenHex, end);
                Debug.Log(path.Count);
                // foreach(GameObject cell in path)
                // {
                //     cell.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.blue;
                // }
                obj.GetComponent<TankMovement>().start_selected = true;
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
