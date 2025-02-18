using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        UpdateValues();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void UpdateValues(){
        CurrencyResponse Currency = await DatabaseCallUtility.FetchUserCurrencies(SessionManager.GetUserID());

        TMPro.TMP_Text PremiumText = transform.Find("PremiumPanel/PremiumText").GetComponent<TMPro.TMP_Text>();
        //TMPro.TMP_Text PremiumText = transform.Find("PremiumPanel").Find("PremiumText").GetComponent<TMPro.TMP_Text>();
        if (PremiumText != null)
            {
                PremiumText.text = $"{Currency.Currency.Premium.ToString()}";
            }

        TMPro.TMP_Text StandardText = transform.Find("StandardPanel/StandardText").GetComponent<TMPro.TMP_Text>();
        //TMPro.TMP_Text StandardText = transform.Find("StandardPanel").Find("StandardText").GetComponent<TMPro.TMP_Text>();
        if (StandardText != null)
            {
                StandardText.text = $"{Currency.Currency.Standard.ToString()}";
            }

    }
}
