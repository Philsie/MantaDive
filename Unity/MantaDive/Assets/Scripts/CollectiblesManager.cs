using Unity.VisualScripting;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    private static CollectiblesManager Instance { get; set; }

    private float primaryCurrency = 0f;
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

    private static CollectiblesManager GetInstance()
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
        var userID = SessionManager.GetUserID();
        var currencies = await DatabaseCallUtility.FetchUserCurrencies(userID);
        primaryCurrency = currencies.Currency.Standard;
        premiumCurrency = currencies.Currency.Premium;
    }

    public static float GetPrimaryCurrency()
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.primaryCurrency;
    }

    public static float SetPrimaryCurrency(int value)
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.primaryCurrency = value;
    }

    public static float ChangePrimaryCurrencyByAmount(int changeValue)
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.primaryCurrency = Instance.primaryCurrency + changeValue;
    }

    public static float GetPremiumCurrency()
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.premiumCurrency;
    }

    public static float SetPremiumCurrency(int value)
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.premiumCurrency = value;
    }

    public static float ChangePremiumCurrencyByAmount(int changeValue)
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.premiumCurrency = Instance.premiumCurrency + changeValue;
    }

}
