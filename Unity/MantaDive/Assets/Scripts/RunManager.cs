using UnityEngine;
using System;

public class RunManager : MonoBehaviour
{
    private static RunManager Instance { get; set; }

    private System.Random randomGenerator;
    private float premiumCurrency = 0f;

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

    private static void SetSeed(int seed)
    {
        GetInstance().randomGenerator = new System.Random(seed);
    }

    private static int GetNextRandomNumber()
    {
        return GetInstance().randomGenerator.Next();
    }
}
