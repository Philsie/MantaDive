using UnityEngine;
using System;

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

    private static RunManager GetInstance()
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
        return GetInstance().randomGenerator.Next();
    }

    public static bool IsRunOngoing()
    {
        return GetInstance().isRunOngoing;
    }

    public static void SetIsRunOngoing(bool isRunOngoing)
    {
        GetInstance().isRunOngoing = isRunOngoing;
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
}
