using System.Threading.Tasks;
using UnityEngine;

public class ExtraStamina : MonoBehaviour, IRunUpgrade
{
    [SerializeField]
    private int stamina = 50;
    [SerializeField]
    private float price = 50;
    private float currentAmount = 0;
    public void ApplyEffect()
    {
        RunManager.ChangeExtraStamina(stamina);
    }

    public async Task<bool> CanBeBought()
    {
        
        return currentAmount >= price;
    }

    public async void DeductCurrency()
    {
        currentAmount -= price;
        await DatabaseCallUtility.UpdateUserPrimaryCurrency(SessionManager.GetUserID(), currentAmount);
    }
    
    public async void TryPurchase()
    {
        CurrencyResponse currencyResponse = await DatabaseCallUtility.FetchUserCurrencies(SessionManager.GetUserID());
        currentAmount = currencyResponse.Currency.Standard;
        if (await CanBeBought())
        {
            DeductCurrency();
            ApplyEffect();
        }
    }
}
