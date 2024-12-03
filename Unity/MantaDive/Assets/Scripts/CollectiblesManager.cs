using Unity.VisualScripting;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    private static CollectiblesManager Instance { get; set; }

    private int primaryCurrency;
    private int premiumCurrency;

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

    private void InitializeCollectibles()
    {
        //TODO: Load value from database once endpoint exists
    }

    public static int GetPrimaryCurrency()
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.primaryCurrency;
    }

    public static int SetPrimaryCurrency(int value)
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.primaryCurrency = value;
    }

    public static int ChangePrimaryCurrencyByAmount(int changeValue)
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.primaryCurrency = Instance.primaryCurrency + changeValue;
    }

    public static int GetPremiumCurrency()
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.premiumCurrency;
    }

    public static int SetPremiumCurrency(int value)
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.premiumCurrency = value;
    }

    public static int ChangePremiumCurrencyByAmount(int changeValue)
    {
        Instance = CollectiblesManager.GetInstance();
        return Instance.premiumCurrency = Instance.premiumCurrency + changeValue;
    }

}
