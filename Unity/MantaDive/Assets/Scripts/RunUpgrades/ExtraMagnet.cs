using System.Threading.Tasks;
using UnityEngine;

public class ExtraMagnet : MonoBehaviour, IRunUpgrade
{
    [SerializeField]
    private int strength = 1;
    [SerializeField]
    private float price = 100;
    private float currentAmount = 0;
    public void ApplyEffect()
    {
        RunManager.ChangeExtraMagnet(strength);
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
