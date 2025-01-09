using System;
using UnityEngine;
using UnityEngine.UI;

public class PopulateShop : MonoBehaviour
{
    [SerializeField]
    private GameObject shopElement;
    private GameObject[] shopElements;
    void Start()
    {
        Populate();
        // Call API for current shop elements
        // Generate shopElements
    }

    private void Populate()
    {
        GameObject element;
        for (int i = 0; i < (shopElements != null ? shopElements.Length : 10); i++)
        {
            element = (GameObject)Instantiate(shopElement, gameObject.transform);
        }

    }

}
