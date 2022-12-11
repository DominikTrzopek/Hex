using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour
{
    public void Leave()
    {
        SceneManager.LoadScene(0);
        TCPConnection.instance.clear();
    }
}
