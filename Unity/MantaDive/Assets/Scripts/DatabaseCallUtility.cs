using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseCallUtility : MonoBehaviour
{
    private static readonly string baseUrl = "http://127.0.0.1:5792/api/";
    private static readonly string userEndpoint = "user/";
    private static readonly string userUpgradesEndpoint = "userUpgrades/";
    private static readonly string userCurrenciesEndpoint = "userCurrencies/";

    private static readonly HttpClient client = new HttpClient();

    private static async Task<bool> SendPatchRequest(string url)
    {
        try
        {
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = new StringContent("")
            };

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Debug.Log($"PATCH successful: {url}");
                return true;
            }
            else
            {
                Debug.LogError($"PATCH failed: {url}, Status Code: {response.StatusCode}");
                return false;
            }
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Request error: {e.Message}");
            return false;
        }
    }

    public static async Task<bool> UpdateUserDailyDepth(int userId, float dailyDepth)
    {
        string url = $"{baseUrl}{userEndpoint}{userId}?DailyDepth={dailyDepth}";
        return await SendPatchRequest(url);
    }

    public static async Task<bool> UpdateUserMaxDepth(int userId, float maxDepth)
    {
        string url = $"{baseUrl}{userEndpoint}{userId}?MaxDepth={maxDepth}";
        return await SendPatchRequest(url);
    }

    public static async Task<bool> UpdateUserTier(int userId, int tier)
    {
        string url = $"{baseUrl}{userEndpoint}{userId}?Tier={tier}";
        return await SendPatchRequest(url);
    }

    public static async Task<bool> UpdateUserName(int userId, string userName)
    {
        string url = $"{baseUrl}{userEndpoint}{userId}?UserName={UnityWebRequest.EscapeURL(userName)}";
        return await SendPatchRequest(url);
    }

    public static async Task<bool> UpdateUserSpeed(int userId, float speed)
    {
        string url = $"{baseUrl}{userUpgradesEndpoint}{userId}?Speed={UnityWebRequest.EscapeURL(speed.ToString("F2", CultureInfo.InvariantCulture))}";
        Debug.Log(url);
        return await SendPatchRequest(url);
    }

    public static async Task<bool> UpdateUserStamina(int userId, float stamina)
    {
        string url = $"{baseUrl}{userUpgradesEndpoint}{userId}?Stamina={UnityWebRequest.EscapeURL(stamina.ToString("F2", CultureInfo.InvariantCulture))}";
        return await SendPatchRequest(url);
    }

    public static async Task<bool> UpdateUserPrimaryCurrency(int userId, float standardCurrency)
    {
        string url = $"{baseUrl}{userCurrenciesEndpoint}{userId}?Standard={UnityWebRequest.EscapeURL(standardCurrency.ToString("F2", CultureInfo.InvariantCulture))}";
        Debug.Log(url);
        return await SendPatchRequest(url);
    }

    public static async Task<bool> UpdateUserPremiumCurrency(int userId, float premiumCurrency)
    {
        string url = $"{baseUrl}{userCurrenciesEndpoint}{userId}?Premium={UnityWebRequest.EscapeURL(premiumCurrency.ToString("F2", CultureInfo.InvariantCulture))}";
        return await SendPatchRequest(url);
    }

    public static async Task<User> FetchUserData(int userId)
    {
        string url = $"{baseUrl}{userEndpoint}{userId}";

        try
        {
            string jsonResponse = await client.GetStringAsync(url);

            Debug.Log($"Response: {jsonResponse}");

            User user = JsonConvert.DeserializeObject<User>(jsonResponse);

            return user;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Request error: {e.Message}");
            return null;
        }
    }

    public static async Task<CurrencyResponse> FetchUserCurrencies(int userId)
    {
        string url = $"{baseUrl}{userCurrenciesEndpoint}{userId}";

        try
        {
            string jsonResponse = await client.GetStringAsync(url);

            Debug.Log($"Response: {jsonResponse}");

            CurrencyResponse currencies = JsonConvert.DeserializeObject<CurrencyResponse>(jsonResponse);

            return currencies;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Request error: {e.Message}");
            return null;
        }
    }
}