using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public bool madeMove = false;
    public bool startSelected = false;

    virtual public void SetPath(List<GameObject> selectedPath){}
    public void SetMadeMove(bool value)
    {
        madeMove = value;
    }

    public bool GetMadeMove()
    {
        return madeMove;
    }

    public bool GetStartSelected()
    {
        return startSelected;
    }

}
