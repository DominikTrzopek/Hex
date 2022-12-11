using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthInfo : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textMeshPro;
    Quaternion rotation;
    void Awake()
    {
        rotation = transform.parent.rotation;
        rotation = Quaternion.Euler(60, 90, rotation.eulerAngles.z);
    }
    void LateUpdate()
    {
        string currentHp = this.transform.root.GetComponent<StatsAbstract>().getHP().ToString();
        string maxHp = this.transform.root.GetComponent<StatsAbstract>().getMaxHP().ToString();
        string level = this.transform.root.GetComponent<StatsAbstract>().getLevel().ToString();
        transform.rotation = rotation;
        if(!this.transform.root.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
            textMeshPro.text = "Lv: " + level + " HP: " + currentHp + "/" + maxHp;
        else
            textMeshPro.text = "HP: " + currentHp + "/" + maxHp;

    }
}
