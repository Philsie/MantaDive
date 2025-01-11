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

            // Find the Title GameObject and set its text
            TMPro.TMP_Text titleText = element.transform.Find("Title").GetComponent<TMPro.TMP_Text>();
            if (titleText != null)
            {
                titleText.text = item.Name;
            }

            TMPro.TMP_Text descriptionText = element.transform.Find("Description").GetComponent<TMPro.TMP_Text>();
            if (descriptionText != null)
            {
                descriptionText.text = item.Description;
            }

            RawImage rawImage = element.transform.Find("RawImage").GetComponent<RawImage>();

            if (rawImage != null){
                Debug.Log("####");
                // Assuming the ShopItem has a Texture2D property called "Image"
                Debug.Log($"Shop_Items/{item.Sprite}");

                string resourcePath = $"Assets/Ressources/Shop_Items/{item.Sprite}";
                Debug.Log($"Attempting to load texture at: {resourcePath}");

                Texture2D texture = LoadTextureFromPath(resourcePath);
                if (texture != null)
                {
                    Debug.Log($"Successfully loaded texture: {resourcePath}");
                    rawImage.texture = texture;
                }
                else
                {
                    Debug.LogError($"Failed to load texture at path: {resourcePath}");
                }
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
