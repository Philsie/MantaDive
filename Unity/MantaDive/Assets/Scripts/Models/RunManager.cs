using UnityEngine;
using System;
using System.Collections;

public class RunManager : MonoBehaviour
{
    private static RunManager Instance { get; set; }

    private System.Random randomGenerator;
    private float standardPremiumCurrency = 0f;
    private float collectedPremiumCurrency = 0f;
    private float currentDepth = 0f;
    private bool isDailyRun = false;
    private bool isRunOngoing = false;
    private bool isGamePaused = false;
    private int extraStamina = 0;
    private int extraMagnet = 0;
    private int baseMagnet = 0;

    public event Action OnRunStateChange;

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

    public static RunManager GetInstance()
    {
        if (Instance == null)
        {
            GameObject singletonObject = new GameObject(nameof(RunManager));
            Instance = singletonObject.AddComponent<RunManager>();
            Instance.randomGenerator = new System.Random();
            DontDestroyOnLoad(singletonObject);
        }

        return Instance;
    }

    public static void SetSeed(int seed)
    {
        GetInstance().randomGenerator = new System.Random(seed);
    }

    public static int GetNextRandomNumber()
    {
        return GetInstance().randomGenerator.Next(1000);
    }

    public static bool IsRunOngoing()
    {
        return GetInstance().isRunOngoing;
    }

    public static void SetIsRunOngoing(bool isRunOngoing)
    {
        GetInstance().isRunOngoing = isRunOngoing;
        GetInstance().OnRunStateChange?.Invoke();
    }

    public static bool IsGamePaused()
    {
        return GetInstance().isGamePaused;
    }

    public static void SetIsGamePaused(bool isGamePaused)
    {
        GetInstance().isGamePaused = isGamePaused;
    }

    public static bool IsDailyRun()
    {
        return GetInstance().isDailyRun;
    }

    public static void SetIsDailyRun(bool isDailyRun)
    {
        GetInstance().isDailyRun = isDailyRun;
    }

    public static float GetCurrentDepth()
    {
        return GetInstance().currentDepth;
    }

    public static void SetCurrentDepth(float newDepth)
    {
        GetInstance().currentDepth = newDepth;
    }

    public static void ChangeCurrentDepth(float changeValue)
    {
        GetInstance().currentDepth = GetInstance().currentDepth + changeValue;
    }

    public static float GetExtraStamina()
    {
        return GetInstance().extraStamina;
    }

    public static void SetExtraStamina(int stamina)
    {
        GetInstance().extraStamina = stamina;
    }

    public static void ChangeExtraStamina(int stamina)
    {
        GetInstance().extraStamina = GetInstance().extraStamina + stamina;
    }

    public static float GetExtraMagnet()
    {
        return GetInstance().extraMagnet;
    }
    public static void SetExtraMagnet(int strength)
    {
        GetInstance().extraMagnet = strength;
        Debug.Log(GetInstance().extraMagnet);
    }
    internal static void ChangeExtraMagnet(int strength)
    {
        GetInstance().extraMagnet = GetInstance().extraMagnet + strength;
    }

    public static float GetBaseMagnet()
    {
        return GetInstance().baseMagnet;
    }
}
