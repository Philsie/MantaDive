using UnityEngine;

public class TestSingletonManagers : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print(PlayerStatsManager.GetPlayerCurrentStamina());
        PlayerStatsManager.ChangePlayerCurrentStaminaByAmount(2);
        print(PlayerStatsManager.GetPlayerCurrentStamina());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
