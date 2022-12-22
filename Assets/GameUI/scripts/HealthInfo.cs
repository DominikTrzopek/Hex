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
        string currentHp = this.transform.root.GetComponent<StatsAbstract>().GetHP().ToString();
        string maxHp = this.transform.root.GetComponent<StatsAbstract>().GetMaxHP().ToString();
        string level = this.transform.root.GetComponent<StatsAbstract>().GetLevel().ToString();
        transform.rotation = rotation;
        if (!this.transform.root.GetComponent<CustomTag>().HasTag(CellTag.mainBase))
            textMeshPro.text = "Lv: " + level + " HP: " + currentHp + "/" + maxHp;
        else
            textMeshPro.text = "HP: " + currentHp + "/" + maxHp;

    }
}
