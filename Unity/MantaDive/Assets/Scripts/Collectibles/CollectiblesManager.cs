using System;
using Unity.VisualScripting;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    private static CollectiblesManager Instance { get; set; }

    private float primaryCurrency = 0f;
    private float premiumCurrency = 0f;

    public event Action<float> OnCurrencyChanged;
    public event Action<float> OnPremiumCurrencyChanged;
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

    public static CollectiblesManager GetInstance()
    {
        if (Instance == null)
        {
            GameObject singletonObject = new GameObject(nameof(CollectiblesManager));
            Instance = singletonObject.AddComponent<CollectiblesManager>();
            Instance.InitializeCollectibles();
            DontDestroyOnLoad(singletonObject);
        }

        return Instance;
    }

    private async void InitializeCollectibles()
    {
        try
        {
            var userID = SessionManager.GetUserID();
            var currencies = await DatabaseCallUtility.FetchUserCurrencies(userID);
            primaryCurrency = currencies.Currency.Standard;
            premiumCurrency = currencies.Currency.Premium;
        }
        catch(Exception e) {
            primaryCurrency = 0;
            premiumCurrency = 0;
        }
    }

    public static float GetPrimaryCurrency()
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.primaryCurrency;
    }
        
    public static float SetPrimaryCurrency(int value)
    {
        Instance = CollectiblesManager.GetInstance();
        Instance.OnCurrencyChanged?.Invoke(value);
        return Instance.primaryCurrency = value;
    }

    public static float ChangePrimaryCurrencyByAmount(int changeValue)
    {
        Instance = CollectiblesManager.GetInstance();
        Instance.primaryCurrency = Instance.primaryCurrency + changeValue;
        Instance.OnCurrencyChanged?.Invoke(Instance.primaryCurrency);
        return Instance.primaryCurrency;
    }

    public static float GetPremiumCurrency()
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.premiumCurrency;
    }

    public static float SetPremiumCurrency(int value)
    {
        Instance = CollectiblesManager.GetInstance();
        Instance.OnPremiumCurrencyChanged?.Invoke(value);
        return Instance.premiumCurrency = value;
    }

    public static float ChangePremiumCurrencyByAmount(int changeValue)
    {
        Instance = CollectiblesManager.GetInstance();
        Instance.premiumCurrency = Instance.premiumCurrency + changeValue;
        Instance.OnPremiumCurrencyChanged?.Invoke(Instance.premiumCurrency);
        return Instance.premiumCurrency;
    }

}
