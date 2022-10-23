using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsLogic : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeCheckmarkVisibility()
    {
        var checkmark = GameObject.Find("Checkmark").GetComponent<Image>();
        Color color = checkmark.color;
        if (color.a != 0f)
            color.a = 0f;
        else
            color.a = 1f;
        checkmark.color = color;
    }

    public void UpdateServerConfig()
    {
        SetServerInputFields.UpdateData();
    }
    
             
}
