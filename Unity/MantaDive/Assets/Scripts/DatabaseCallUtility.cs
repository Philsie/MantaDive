using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseCallUtility : MonoBehaviour
{
    private static readonly string baseUrl = "http://127.0.0.1:5792/api/";
    private static readonly string userEndpoint = "user/";
    private static readonly string userUpgradesEndpoint = "userUpgrades/";
    private static readonly string userCurrenciesEndpoint = "userCurrencies/";
    private static readonly string dailySeedEndpoint = "getSeed/";
    private static readonly string leaderboardEndpoint = "getLeaderboard/";
    private static readonly string dailyLeaderboardEndpoint = "getDailyLeaderboard/";
    private static readonly string unlockShopItemEndpoint = "unlockShopItem/";
    private static readonly string getSingleShopItemEndpoint = "getShopItem/";
    private static readonly string getAvailableShopItemsEndpoint = "getAvailableShopItems/";

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
            CurrencyResponse currencies = JsonConvert.DeserializeObject<CurrencyResponse>(jsonResponse);
            return currencies;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Request error: {e.Message}");
            return null;
        }
    }

    public static async Task<int> FetchDailySeed()
    {

        string[] date = DateTime.Now.ToString("yyyy_MM_dd").Split('_');
        date[1] = int.Parse(date[1]).ToString();
        date[2] = int.Parse(date[2]).ToString();
        string fixedDate = string.Join("_", date);
        
        string url = $"{baseUrl}{dailySeedEndpoint}{fixedDate}";

        try
        {
            string jsonResponse = await client.GetStringAsync(url);
            Debug.Log(jsonResponse);
            JObject jsonObject = JObject.Parse(jsonResponse);
            int seed = (int)jsonObject["Value"];

            return seed;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Request error: {e.Message}");
            return 0;
        }
    }

    public static async Task<List<UserDepth>> FetchLeaderboardSpots(int numOfSpots)
    {

        string url = $"{baseUrl}{leaderboardEndpoint}{numOfSpots}";
        Debug.Log($"URL: {url}");

        try
        {
            string jsonResponse = await client.GetStringAsync(url);
            List<UserDepth> userDepths = JsonConvert.DeserializeObject<List<UserDepth>>(jsonResponse);
            return userDepths;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Request error: {e.Message}");
            return null;
        }
    }

    public static async Task<List<UserDepth>> FetchDailyLeaderboardSpots(int numOfSpots)
    {

        string url = $"{baseUrl}{dailyLeaderboardEndpoint}{numOfSpots}";
        Debug.Log($"URL: {url}");

        try
        {
            string jsonResponse = await client.GetStringAsync(url);
            string updatedJson = ReplaceJsonKey(jsonResponse, "DailyDepth", "MaxDepth");
            List<UserDepth> userDepths = JsonConvert.DeserializeObject<List<UserDepth>>(updatedJson);
            return userDepths;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Request error: {e.Message}");
            return null;
        }
    }

    public static async Task<ShopItem> FetchShopItem(int shopItemID)
    {

        string url = $"{baseUrl}{getSingleShopItemEndpoint}{shopItemID}";

        try
        {
            string jsonResponse = await client.GetStringAsync(url);
            ShopItem shopItem = JsonConvert.DeserializeObject<ShopItem>(jsonResponse);
            return shopItem;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Request error: {e.Message}");
            return null;
        }
    }

    public static async Task<List<ShopItem>> FetchAvailableShopItems(int userID)
    {

        string url = $"{baseUrl}{getAvailableShopItemsEndpoint}{userID}";
        Debug.Log(url);

        try
        {
            string jsonResponse = await client.GetStringAsync(url);
            List<ShopItem> shopItems = JsonConvert.DeserializeObject<List<ShopItem>>(jsonResponse);
            return shopItems;
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Request error: {e.Message}");
            return null;
        }
    }

    public static async Task<bool> UnlockShopItemForUser(int userId, int shopItemId)
    {
        string url = $"{baseUrl}{unlockShopItemEndpoint}{userId}/{shopItemId}";
        return await SendPatchRequest(url);
    }


    public static async Task<bool> PostLevelMetaData(int userId, float timeElapsed, int shotsFired, int enemiesHit, int coinsCollected)
    {
        string url = $"{baseUrl}levelMetadata/{userId}";
        var payload = new
        {
            TimeElapsed = timeElapsed,
            ShotsFired = shotsFired,
            EnemiesHit = enemiesHit,
            CoinsCollected = coinsCollected
        };
        Debug.Log(payload.ToString());
        Debug.Log(url);
        string jsonPayload = JsonConvert.SerializeObject(payload);

        try
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Debug.Log("Level metadata posted successfully!");
                return true;
            }
            else
            {
                Debug.LogError($"Failed to post level metadata. Status Code: {response.StatusCode}");
                return false;
            }
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"Request error: {e.Message}");
            return false;
        }
    }


    private static string ReplaceJsonKey(string json, string oldKey, string newKey)
    {
        var objects = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
        foreach (var obj in objects)
        {
            if (obj.ContainsKey(oldKey))
            {
                obj[newKey] = obj[oldKey];
                obj.Remove(oldKey);
            }
        }
        return JsonConvert.SerializeObject(objects, Formatting.Indented);
    }



}