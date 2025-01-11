using System;
using System.Reflection;
using UnityEngine;
using System.Threading.Tasks;

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

        TMPro.TMP_Text PremiumText = transform.Find("PremiumText").GetComponent<TMPro.TMP_Text>();
        if (PremiumText != null)
            {
                PremiumText.text = $"Premium: {Currency.Currency.Premium.ToString()}";
            }

        TMPro.TMP_Text StandardText = transform.Find("StandardText").GetComponent<TMPro.TMP_Text>();
        if (StandardText != null)
            {
                StandardText.text = $"Standard: {Currency.Currency.Standard.ToString()}";
            }

    }



}
