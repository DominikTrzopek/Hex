using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerObj : MonoBehaviour
{
    public GameObject uiImage;
    public GameObject unit;
    public GameObject baseUiPanel;
    public GameObject unitUiPanel;
    private Vector3 basePosition;

    private CommandEnum command;

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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayhitStart, Mathf.Infinity, layerPlayer))
            {
                GameObject obj = rayhitStart.collider.transform.parent.gameObject;
                Debug.Log(obj.name);
                if (obj.GetComponent<NetworkId>().ownerId == UDPServerConfig.getId())
                {
                    if (obj.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
                    {
                        BaseActions.obj = obj;
                        BaseActions.uiImage = uiImage;
                        baseUiPanel.SetActive(true);
                        basePosition = obj.transform.position;
                        command = CommandEnum.INSTANTIANE_UNIT;
                    }
                    else if(obj.GetComponent<CustomTag>().HasTag(CellTag.player))
                    {
                        Debug.Log(obj.name);
                        
                    }
                }
            }

            if(command == CommandEnum.INSTANTIANE_UNIT)
                HandleInstantiateUnitCommand();
            

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
            }
        }
    }




}
