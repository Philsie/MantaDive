using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PopulateShop : MonoBehaviour
{
    [SerializeField]
    private GameObject shopElement;
    private GameObject[] shopElements;
    private List<ShopItem> availableShopElements;
    void Start()
    {
        Populate();
        // Call API for current shop elements
        // Generate shopElements
    }

    private async void Populate()
    {
        // Fetch available shop items
        availableShopElements = await DatabaseCallUtility.FetchAvailableShopItems(SessionManager.GetUserID());

        foreach (ShopItem item in availableShopElements)
        {
            // Instantiate the shop element prefab
            GameObject element = Instantiate(shopElement, transform);

            ShopElement script = element.GetComponent<ShopElement>();
            script.Id = item.ID;

            // Find the Title GameObject and set its text
            TMPro.TMP_Text titleText = element.transform.Find("ShopElementNormal/Title").GetComponent<TMPro.TMP_Text>();
            if (titleText != null)
            {
                string titleWithPrice = item.Name;

                if (item.Price.Standard != 0){
                    titleWithPrice += $"\n(Std.: {item.Price.Standard})";
                } else if (item.Price.Premium != 0){
                    titleWithPrice += $"\n(Prem.: {item.Price.Premium})";
                } else {
                    Debug.LogError($"No Price set for Item with ID-{item.ID}");
                }


                titleText.text = titleWithPrice;
            }

            TMPro.TMP_Text descriptionText = element.transform.Find("ShopElementNormal/Description").GetComponent<TMPro.TMP_Text>();
            if (descriptionText != null)
            {
                descriptionText.text = item.Description;
            }

            RawImage rawImage = element.transform.Find("ShopElementNormal/RawImage").GetComponent<RawImage>();

            if (rawImage != null){

                string resourcePath = $"Assets/Ressources/Shop_Items/{item.Sprite}";

                Texture2D texture = LoadTextureFromPath(resourcePath);
                if (texture != null)
                {
                    rawImage.texture = texture;
                }
                else
                {
                    Debug.LogError($"Failed to load texture at path: {resourcePath}");
                }
            }

            CurrencyResponse Currency = await DatabaseCallUtility.FetchUserCurrencies(SessionManager.GetUserID());
            bool disabled = false;

            if (item.Price.Premium > 0 && Currency.Currency.Premium < item.Price.Premium){
                disabled = true;
            }
            else if (item.Price.Standard > 0 && Currency.Currency.Standard < item.Price.Standard){
                disabled = true;
            }

            if (disabled == true){
                Image normalPanel = element.transform.Find("ShopElementNormal").GetComponent<Image>();
                ColorUtility.TryParseHtmlString("#3131317F", out Color disabledPanel);
                normalPanel.color = disabledPanel;
                descriptionText.color = Color.white;
                titleText.color = Color.white;
                script.disabledPopup = true;
            }

            TMPro.TMP_Text ConfirmText = element.transform.Find("ShopElementConfirmPopup/ConfirmText").GetComponent<TMPro.TMP_Text>();
            if (ConfirmText != null)
            {
                ConfirmText.text = $"Do you really want to buy {item.Name}?";
            }
        }
    }

    static public Texture2D LoadTextureFromPath(string fPath){
    {
      Texture2D texture = null;
      byte[] fData;

      if(File.Exists(fPath)){
        fData = File.ReadAllBytes(fPath);
        texture = new Texture2D(1,1); // we dont care about size
        texture.LoadImage(fData); //load image from direct bytes
      }
      else{
        //handle graceful fails here
      }
      return texture;
    }

    }
}
