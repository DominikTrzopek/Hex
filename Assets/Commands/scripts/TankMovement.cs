using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{

    //****************************************************************************************************************


    static public void Move(GameObject player)
    {
        //-----------------------------------------------------------------------------------
        Rigidbody obj = player.GetComponent<Rigidbody>();
        GameObject global_pivot = player.transform.GetChild(0).gameObject;
        Vector3 direction = (global_pivot.transform.position - player.transform.position).normalized;
        //-----------------------------------------------------------------------------------
        obj.AddForce(direction);   
    }

    //****************************************************************************************************************

    public int maxrange = 5;
    public float maxvelocity = 1;
    List<GameObject> list_dup = new List<GameObject>();
    public List<GameObject> path = new List<GameObject>();
    List<GameObject> obj_in_range = new List<GameObject>();
    public bool path_selected = false;
    public bool start_selected = false;
    public bool moving = false;
    LayerMask layer_start, layer_end;
    GameObject start, end;
    bool start_moving = false;
    bool start_rotating = false;
    public int click_count;
    public float speed;
    
    private void Start()
    {
        layer_start = LayerMask.GetMask("Player");
        layer_end = LayerMask.GetMask("Default");      
    }

    public void setPath(List<GameObject> objects)
    {
        path = objects;
    }

    //****************************************************************************************************************

    private void Update()
    {
        //-----------------------------------------------------------------------------------
        Rigidbody obj = gameObject.GetComponent<Rigidbody>();
        GameObject global_pivot = gameObject.transform.GetChild(0).gameObject;
        Vector3 direction = (global_pivot.transform.position - gameObject.transform.position).normalized;
        //-----------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------
        if (start_selected == true && path.Count > 0)
        {
            click_count++;
            if(click_count == 1)
            {
                PathFinding.ClearDistance(obj_in_range);
                obj_in_range.Clear();
                end = path[path.Count - 1];
                end.GetComponent<CustomTag>().taken = true;
                // end.GetComponent<CustomTag>().taken_by_player = true;
                // Str_container.taken.Add(end);
                moving = true;
                path_selected = true;
                start_rotating = true;
            }

        }
        //-----------------------------------------------------------------------------------
        if (start_rotating == true)
        {
            Vector3 current_position = path[0].transform.position - path[1].transform.position;
            current_position = new Vector3(current_position.x, 0, current_position.z);
            float rotation = HexMetrics.GetRotation(current_position);
            Quaternion to = Quaternion.Euler (0, rotation, 0);
            if(Vector3.Distance(transform.eulerAngles, to.eulerAngles) > 0.01f)
            {
                obj.velocity = new Vector3(0, 0, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 1);
            }
            else
            {
                start_rotating = false;
                start_moving = true;      
            }
        }
        //-----------------------------------------------------------------------------------
        if (start_moving == true)
        {
            float distance = HexMetrics.innerRadious * 4;
            float current_distance = CalculateDistance(gameObject, path[0]);
            if(obj.velocity.magnitude < maxvelocity)
            {
                obj.AddForce(direction * speed);
                Debug.Log(direction);
            }
            else if(current_distance > distance)
            {
                path.Remove(path[0]);
                if(path.Count > 1)
                {
                    start_rotating = true;
                }
                else
                {
                    obj.velocity = new Vector3(0, 0, 0);
                }
                start_moving = false;   
            } 
        }
        if(end != null)
            if (path.Count <= 1)
                moving = false;
        if(path_selected == true && moving == false)
        {
            var script = obj.GetComponent<TankMovement>();
            script.enabled = false;
        }
    }

    //****************************************************************************************************************

    static float CalculateDistance(GameObject a, GameObject b)
    {
        float x = a.transform.position.x - b.transform.position.x;
        float y = a.transform.position.z - b.transform.position.z;
        return (Mathf.Sqrt(x * x + y * y));
    }

    //****************************************************************************************************************

}
