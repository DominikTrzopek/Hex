using UnityEngine;

public class PrefabContainer : MonoBehaviour
{
    public static PrefabContainer container;
    public GameObject playerUnitPrefab;
    public GameObject structurePrefab;
    public GameObject roadPrefab;
    public GameObject bottomPanel;

    private void Awake()
    {
        if (container != null && container != this)
        {
            Destroy(this);
        }
        else
        {
            container = this;
        }
    }
}
