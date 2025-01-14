using UnityEngine;

public class InstantiateManagers : MonoBehaviour
{
    private void Awake()
    {
        PlayerStatsManager.GetInstance();
        RunManager.GetInstance();
        CollectiblesManager.GetInstance();
    }
}
