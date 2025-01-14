using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
public class PlayerStatsManager : MonoBehaviour
{
    private static PlayerStatsManager Instance { get; set; }

    private static PlayerStats playerDefaultStats;

    private float playerMaxStamina;
    private float playerCurrentStamina;

    private float playerBaseSpeed;
    private float playerCurrentSpeed;

    private float playerMagnetStrength;
    private float playerDepth = 0;

    // Events for stat changes
    public event Action<float> OnStaminaChanged;
    public event Action<float> OnSpeedChanged;
    public event Action<float> OnMagnetChanged;
    public event Action<float> OnDepthChanged;


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

    public static PlayerStatsManager GetInstance()
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

    private async void InitializePlayerStats()
    {
        //TODO: Load player stats from DB based on userID

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

        var userID = SessionManager.GetUserID();
        User user = await DatabaseCallUtility.FetchUserData(userID);

        if (user != null)
        {
            if (user.Upgrades.TryGetValue("Speed", out float speed))
            {
                playerBaseSpeed = speed;
                playerCurrentSpeed = speed;
            }
            else
            {
                Debug.LogError("Speed not found in upgrades.");
            }

            if (user.Upgrades.TryGetValue("Stamina", out float stamina))
            {
                playerMaxStamina = stamina;
                playerCurrentStamina = stamina;
            }
            else
            {
                Debug.LogError("Stamina not found in upgrades.");
            }
        }
        else
        {
            Debug.LogError("Failed to fetch user data. Setting dummy values");

            //Dummy valuess
            playerCurrentStamina = 100;
            playerBaseSpeed = 3;
            playerCurrentSpeed = 3;
            playerMagnetStrength = 5;
        }
        OnStaminaChanged?.Invoke(playerCurrentStamina);
        OnSpeedChanged?.Invoke(playerCurrentSpeed);
        OnMagnetChanged?.Invoke(playerMagnetStrength);
    }

    public static float GetPlayerCurrentStamina()
    {
        Instance = PlayerStatsManager.GetInstance();
        return Instance.playerCurrentStamina;
    }
    public static float SetPlayerCurrentStamina(float value)
    {
        Instance = PlayerStatsManager.GetInstance();
        Instance.OnStaminaChanged?.Invoke(value);
        return Instance.playerCurrentStamina = value;
    }

    public static float ChangePlayerCurrentStaminaByAmount(float changeValue)
    {
        Instance = PlayerStatsManager.GetInstance();
        Instance.playerCurrentStamina = Instance.playerCurrentStamina + changeValue;
        Instance.OnStaminaChanged?.Invoke(Instance.playerCurrentStamina);
        return Instance.playerCurrentStamina;
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
        Instance.OnMagnetChanged?.Invoke(value);
        return Instance.playerMagnetStrength = value;
    }

    public static float ChangePlayerMagnetStrengthByAmount(float changeValue)
    {
        Instance = PlayerStatsManager.GetInstance();
        Instance.playerMagnetStrength = Instance.playerMagnetStrength + changeValue;
        Instance.OnMagnetChanged?.Invoke(Instance.playerMagnetStrength);
        return Instance.playerMagnetStrength;
    }

    public static float GetPlayerDepth()
    {
        return Instance.playerDepth;
    }
    public static float SetPlayerDepth(float value)
    {
        Instance.playerDepth = value;
        Instance.OnDepthChanged?.Invoke(value);
        return Instance.playerDepth;
    }
    public static float ChangePlayerDepthByValue(float value)
    {
        Instance.playerDepth += value;
        Instance.OnDepthChanged?.Invoke(Instance.playerDepth);
        return Instance.playerDepth;
    }

    public static IEnumerator TempUpdateSpeed(float speedUpdate, float time)
    {
        ChangePlayerCurrentSpeedByAmount(speedUpdate);
        yield return new WaitForSeconds(time);
        ChangePlayerCurrentSpeedByAmount(-speedUpdate);
    }

    public static IEnumerator TempUpdateMagnet(float magnetUpdate, float time)
    {
        ChangePlayerMagnetStrengthByAmount(magnetUpdate);
        yield return new WaitForSeconds(time);
        ChangePlayerMagnetStrengthByAmount(-magnetUpdate);
    }

    public static void RefreshLevelUI()
    {
        Instance.OnDepthChanged?.Invoke(Instance.playerDepth);
        Instance.OnMagnetChanged?.Invoke(Instance.playerMagnetStrength);
        Instance.OnStaminaChanged?.Invoke(Instance.playerCurrentStamina);
    }

}
