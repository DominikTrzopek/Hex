using UnityEngine;

public class Costs : MonoBehaviour
{
    public static Costs container;
    public int initUnit;
    public int initStructure;
    public int makeBank;
    public int upgradeGun;
    public int upgradeChasis;
    public int upgradeEngine;
    public int upgradeRadio;
    public int upgradeStructure;

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
