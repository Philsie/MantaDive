using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ExtraMagnet : MonoBehaviour, IRunUpgrade
{
    [SerializeField]
    private int strength = 1;
    [SerializeField]
    private float price = 100;
    private float currentAmount = 0;
    [SerializeField]
    private TMP_Text coins;
    [SerializeField]
    private string textCoins = "0";
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
        textCoins = currentAmount.ToString();
        coins.text = textCoins;
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
