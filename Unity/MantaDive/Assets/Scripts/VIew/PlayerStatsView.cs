using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text currencyText;
    [SerializeField]
    private TMP_Text premiumCurrencyText;
    [SerializeField]
    private TMP_Text staminaText;
    [SerializeField]
    private TMP_Text magnetStrengthText;
    [SerializeField]
    private TMP_Text speedText;

    private void OnEnable()
    {
        PlayerStatsManager.GetInstance().OnStaminaChanged += UpdateStaminaText;
        PlayerStatsManager.GetInstance().OnSpeedChanged += UpdateSpeedText;
        PlayerStatsManager.GetInstance().OnMagnetChanged += UpdateMagnetText;
        CollectiblesManager.GetInstance().OnCurrencyChanged += UpdateCurrencyText;
        CollectiblesManager.GetInstance().OnPremiumCurrencyChanged += UpdatePremiumCurrencyText;
    }
    private void OnDisable()
    {
        PlayerStatsManager.GetInstance().OnStaminaChanged -= UpdateStaminaText;
        PlayerStatsManager.GetInstance().OnSpeedChanged -= UpdateSpeedText;
        PlayerStatsManager.GetInstance().OnMagnetChanged -= UpdateMagnetText;
        CollectiblesManager.GetInstance().OnCurrencyChanged -= UpdateCurrencyText;
        CollectiblesManager.GetInstance().OnPremiumCurrencyChanged -= UpdatePremiumCurrencyText;
    }

    private void UpdateStaminaText(float stamina)
    {
        staminaText.text = $"S: {stamina}";
    }
    private void UpdateMagnetText(float magnet)
    {
        magnetStrengthText.text = $"M: {magnet}";
    }
    private void UpdateSpeedText(float speed)
    {
    }
    private void UpdateCurrencyText(float currency)
    {
        currencyText.text = $"${currency}";
    }
    private void UpdatePremiumCurrencyText(float premium)
    {
        premiumCurrencyText.text = $"$${premium}";
    }
}
