using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public bool madeMove = false;
    public bool inAction = false;

    virtual public void SetEnemy(GameObject newEnemy){}
    public void SetMadeMove(bool value)
    {
        madeMove = value;
    }

    public bool GetMadeMove()
    {
        return madeMove;
    }

    public bool GetInAction()
    {
        return inAction;
    }
}
