using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenu : MonoBehaviour
{
    public void Leave()
    {
        SceneManager.LoadScene(0);
        Resources.Clear();
        PlayerActionSelector.command = CommandEnum.NONE;
        TCPConnection.instance.clear();
    }
}
