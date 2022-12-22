using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelHolder : MonoBehaviour
{
    public List<GameObject> panels;
    public GameObject tooltip;
    public TMPro.TextMeshProUGUI textMeshPro;
    public GameObject bottomPanel;
    public Button endTurnButton;
    public GameObject endGamePannel;
    public static PanelHolder holder;
    private void Awake()
    {
        if (holder != null && holder != this)
        {
            Destroy(this);
        }
        else
        {
            holder = this;
        }
    }

    public void DisableUiPlanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }
}
