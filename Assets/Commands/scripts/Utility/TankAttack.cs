using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttack : MonoBehaviour
{
    public GameObject pivot;
    GameObject enemy;
    public float speed = 1f;
    Vector3 target;
    GameObject turret;
    bool start = false;
    float newRotation;


    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
        turret = this.transform.Find("turret").gameObject;
        newRotation = turret.transform.rotation.y + 10f;
        start = true;
    }


    void Update()
    {
        
        if(start == true)
        {   
            float singleStep = speed * Time.deltaTime;
            target = enemy.transform.position - turret.transform.position;
            target.y = 0;
            Vector3 rotation = Vector3.RotateTowards(turret.transform.forward, target, singleStep, 0.0f);
            turret.transform.rotation = Quaternion.LookRotation(rotation);
            if(Mathf.Abs(turret.transform.rotation.y - newRotation) <= 0.0001f)
                start = false;
            newRotation = turret.transform.rotation.y;
        }
        else
        {
            float singleStep = speed * Time.deltaTime;
            target = pivot.transform.position - turret.transform.position;
            target.y = 0;
            Vector3 rotation = Vector3.RotateTowards(turret.transform.forward, target, singleStep, 0.0f);
            turret.transform.rotation = Quaternion.LookRotation(rotation);
            if(Mathf.Abs(turret.transform.rotation.y - newRotation) <= 0.0001f)
                start = false;
            newRotation = turret.transform.rotation.y;
        }

    }
}
