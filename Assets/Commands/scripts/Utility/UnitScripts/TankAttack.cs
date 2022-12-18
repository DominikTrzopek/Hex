using UnityEngine;
using System;

public class TankAttack : Attack
{
    const float threshold = 0.00001f;
    public GameObject pivot;
    public LineRenderer lineRenderer;
    public GameObject gunPivot;
    public GameObject turret;
    GameObject enemy;
    public float speed = 1f;
    public float widthStep = 0.001f;
    public float maxWidth = 0.01f;
    Vector3 target;
    bool start = false;
    bool attack = false;
    bool increment = true;
    float newRotation;
    int iteration = 1;

    override public void SetEnemy(GameObject newEnemy)
    {
        if (attack || start)
            enemy.GetComponent<StatsAbstract>().ApplyReceivedAttack();

        inAction = true;
        attack = false;
        increment = true;
        iteration = 1;
        ActionCounters.isAttackingCount++;
        this.enemy = newEnemy;
        newRotation = turret.transform.rotation.y + 10f;
        start = true;
        if (this.GetComponent<NetworkId>().ownerId == UDPServerConfig.getId())
        {
            madeMove = true;
            this.gameObject.GetComponent<TankMovement>().madeMove = true;
        }
    }

    void FixedUpdate()
    {
        if(enemy == null && start)
            RotateTurret(SetBoolsAfterAttack, pivot);
        if (start)
            RotateTurret(SetBoolsBeforeAttack, enemy);
        else if (attack)
            Attack();
        else if (!start && !attack && inAction)
            RotateTurret(SetBoolsAfterAttack, pivot);
    }

    private void Attack()
    {
        float width = iteration * widthStep;
        SetLinePoints();
        SetIteration(width);
        if (width == 0)
        {
            attack = false;
            enemy.GetComponent<StatsAbstract>().ApplyReceivedAttack();
        }
        SetLineWidth(width);
    }

    private void SetLinePoints()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, gunPivot.transform.position);
        lineRenderer.SetPosition(1, enemy.transform.position);
    }

    private void SetLineWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    private void SetIteration(float width)
    {
        if (width <= maxWidth && increment == true)
            iteration++;
        else
        {
            iteration--;
            increment = false;
        }
    }

    private void RotateTurret(Action func, GameObject target)
    {
        Vector3 rotation = GetRotationVector(target);
        turret.transform.rotation = Quaternion.LookRotation(rotation);
        if (Mathf.Abs(turret.transform.rotation.y - newRotation) <= threshold)
            func();
        newRotation = turret.transform.rotation.y;
    }

    private Vector3 GetRotationVector(GameObject targetObj)
    {
        float singleStep = speed * Time.deltaTime;
        target = targetObj.transform.position - turret.transform.position;
        target.y = 0;
        return Vector3.RotateTowards(turret.transform.forward, target, singleStep, 0.0f);
    }

    private void SetBoolsBeforeAttack()
    {
        attack = true;
        start = false;
    }

    private void SetBoolsAfterAttack()
    {
        start = false;
        inAction = false;
        ActionCounters.isAttackingCount--;
    }
}
