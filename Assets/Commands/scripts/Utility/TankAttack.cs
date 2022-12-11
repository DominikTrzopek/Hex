using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttack : MonoBehaviour
{
    public GameObject pivot;
    public LineRenderer lineRenderer;
    public GameObject gunPivot;
    GameObject enemy;
    public float speed = 1f;
    public float widthStep = 0.001f;
    public float maxWidth = 0.01f;
    Vector3 target;
    GameObject turret;
    bool start = false;
    bool attack = false;
    bool increment = true;
    float newRotation;


    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
        turret = this.transform.Find("turret").gameObject;
        newRotation = turret.transform.rotation.y + 10f;
        start = true;
    }

    int iteration = 1;
    void FixedUpdate()
    {
        if (start == true)
        {
            float singleStep = speed * Time.deltaTime;
            target = enemy.transform.position - turret.transform.position;
            target.y = 0;
            Vector3 rotation = Vector3.RotateTowards(turret.transform.forward, target, singleStep, 0.0f);
            turret.transform.rotation = Quaternion.LookRotation(rotation);
            if (Mathf.Abs(turret.transform.rotation.y - newRotation) <= 0.001f)
            {
                attack = true;
                start = false;
            }
            newRotation = turret.transform.rotation.y;
        }
        else if (attack == true)
        {
            float width = iteration * widthStep;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, gunPivot.transform.position);
            lineRenderer.SetPosition(1, enemy.transform.position);
            if (width <= maxWidth && increment == true)
            {
                iteration++;
            }
            else
            {
                iteration--;
                increment = false;
            }
            if (width == 0)
            {
                attack = false;
                increment = true;
                iteration = 1;
                enemy.GetComponent<StatsAbstract>().applyAttackPoints(this.GetComponent<UnitStats>().getAP());
            }
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
        }
        else if (start == false && attack == false)
        {
            float singleStep = speed * Time.deltaTime;
            target = pivot.transform.position - turret.transform.position;
            target.y = 0;
            Vector3 rotation = Vector3.RotateTowards(turret.transform.forward, target, singleStep, 0.0f);
            turret.transform.rotation = Quaternion.LookRotation(rotation);
            if (Mathf.Abs(turret.transform.rotation.y - newRotation) <= 0.0001f)
                this.enabled = false;
            newRotation = turret.transform.rotation.y;
        }

    }
}
