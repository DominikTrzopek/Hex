using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject panel;
    public TMPro.TextMeshProUGUI textMeshPro;
    public string text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
        textMeshPro.text = text;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
        textMeshPro.text = "";
    }
}
