using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : Movement
{
    public float maxVelocity = 1;
    public float speed;
    public bool pathSelected = false;
    public bool moving = false;
    List<GameObject> path = new List<GameObject>();
    GameObject end;
    bool startMoving = false;
    bool startRotating = false;

    override public void SetPath(List<GameObject> selectedPath)
    {
        path = selectedPath;
        startSelected = true;
        ActionCounters.isMovingCount++;
        if (this.GetComponent<NetworkId>().ownerId == UDPServerConfig.getId())
            madeMove = true;
    }

    private void Update()
    {
        Rigidbody obj = gameObject.GetComponent<Rigidbody>();
        GameObject pivot = gameObject.transform.GetChild(0).gameObject;
        Vector3 direction = (pivot.transform.position - gameObject.transform.position).normalized;

        if (startSelected == true && path.Count > 0)
        {
            end = path[path.Count - 1];
            end.GetComponent<CustomTag>().taken = true;
            moving = true;
            pathSelected = true;
            startRotating = true;
        }

        if (startRotating == true)
        {
            Vector3 currentPosition = path[0].transform.position - path[1].transform.position;
            currentPosition.y = 0f;
            float rotation = HexMetrics.GetRotation(currentPosition);
            Quaternion to = Quaternion.Euler(0, rotation, 0);
            if (Vector3.Distance(transform.eulerAngles, to.eulerAngles) > 0.01f)
            {
                obj.velocity = new Vector3(0, 0, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 1);
            }
            else
            {
                startRotating = false;
                startMoving = true;
            }
        }

        if (startMoving == true)
        {
            float distance = HexMetrics.innerRadious * 4;
            float current_distance = CalculateDistance(gameObject, path[0]);
            if (obj.velocity.magnitude < maxVelocity)
            {
                obj.AddForce(direction * speed);
            }
            if (current_distance > distance)
            {
                obj.transform.position = path[1].transform.position;
                path.Remove(path[0]);
                if (path.Count > 1)
                {
                    startRotating = true;
                }
                else
                {
                    obj.velocity = new Vector3(0, 0, 0);
                }
                startMoving = false;
            }
        }
        if (end != null)
            if (path.Count <= 1)
                moving = false;
        if (pathSelected == true && moving == false)
        {
            var script = obj.GetComponent<TankMovement>();
            if(startSelected == true)
                ActionCounters.isMovingCount--;
            startSelected = false;
        }
    }

    static float CalculateDistance(GameObject a, GameObject b)
    {
        float x = a.transform.position.x - b.transform.position.x;
        float y = a.transform.position.z - b.transform.position.z;
        return (Mathf.Sqrt(x * x + y * y));
    }
}
