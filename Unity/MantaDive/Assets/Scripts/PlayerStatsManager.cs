using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    private static PlayerStatsManager Instance { get; set; }

    private static PlayerStats playerDefaultStats;

    private float playerMaxStamina;
    private float playerCurrentStamina;

    private float playerBaseSpeed;
    private float playerCurrentSpeed;

    private float playerMagnetStrength;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private static PlayerStatsManager GetInstance()
    {
        if (Instance == null)
        {
            GameObject singletonObject = new GameObject(nameof(PlayerStatsManager));
            Instance = singletonObject.AddComponent<PlayerStatsManager>();
            Instance.InitializePlayerStats();
            DontDestroyOnLoad(singletonObject);
        }

        return Instance;
    }

    private void InitializePlayerStats()
    {
        if (playerDefaultStats == null)
        {
            playerDefaultStats = AssetDatabase.LoadAssetAtPath<PlayerStats>("Assets/ScriptableObjects/PlayerStats.asset");
            if (playerDefaultStats != null)
            {
                playerCurrentStamina = playerDefaultStats.playerStamina;
                playerMaxStamina = playerDefaultStats.playerStamina;

                playerBaseSpeed = playerDefaultStats.playerSpeed;
                playerCurrentSpeed = playerDefaultStats.playerSpeed;

                playerMagnetStrength = playerDefaultStats.magnetRadius;
            }
            else
            {
                Debug.LogError("PlayerStats ScriptableObject not found in Resources!");
            }
        }
    }

    public static float GetPlayerCurrentStamina()
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerCurrentStamina;
    }
    public static float SetPlayerCurrentStamina(float value)
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerCurrentStamina = value;
    }

    public static float ChangePlayerCurrentStaminaByAmount(float changeValue)
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerCurrentStamina = Instance.playerCurrentStamina + changeValue;
    }

    public static float GetPlayerMaxStamina()
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerMaxStamina;
    }

    public static float SetPlayerMaxStamina(float value)
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerMaxStamina = value;
    }

    public static float ChangePlayerMaxStaminaByAmount(float changeValue)
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerMaxStamina = Instance.playerMaxStamina + changeValue;
    }

    public static float GetPlayerBaseSpeed()
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerBaseSpeed;
    }

    public static float SetPlayerBaseSpeed(float value)
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerBaseSpeed = value;
    }

    public static float ChangePlayerBaseSpeedByAmount(float changeValue)
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerBaseSpeed = Instance.playerBaseSpeed + changeValue;
    }

    public static float GetPlayerCurrentSpeed()
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerCurrentSpeed;
    }

    public static float SetPlayerCurrentSpeed(float value)
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerCurrentSpeed = value;
    }

    public static float ChangePlayerCurrentSpeedByAmount(float changeValue)
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerCurrentSpeed = Instance.playerCurrentSpeed + changeValue;
    }

    public static float GetPlayerMagnetStrength()
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerMagnetStrength;
    }

    public static float SetPlayerMagnetStrength(float value)
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerMagnetStrength = value;
    }

}
