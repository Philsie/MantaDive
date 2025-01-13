using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField]
    private GameObject leaderboardElement;
    private List<UserDepth> overallSpots = new List<UserDepth>();
    private List<UserDepth> dailySpots = new List<UserDepth>();
    private bool isDailyShown = false;
    private bool isOverallShown = false;
    //Color disabledButton = ColorUtility.TryParseHtmlString("#3131317F", out Color disabledButton);
    Color32 disabledButton = new Color32 (49,49,49,128);
    Color32 enabledButton = new Color32 (189,227,255,128);

    public void PopulateOverall()
    {
        if (isOverallShown) return;
        StartCoroutine(WaitForSpots(false));
        isDailyShown = false;
        isOverallShown = true;
        GameObject.Find("Daily").GetComponent<UnityEngine.UI.Image>().color =  enabledButton ;
        GameObject.Find("Overall").GetComponent<UnityEngine.UI.Image>().color = disabledButton ;
        GameObject.Find("Subtitle").GetComponent<TMPro.TMP_Text>().text = "Overall";
    }
    public void PopulateDaily()
    {
        if (isDailyShown) return;
        StartCoroutine(WaitForSpots(true));
        isDailyShown = true;
        isOverallShown = false;
        GameObject.Find("Overall").GetComponent<UnityEngine.UI.Image>().color = enabledButton ;
        GameObject.Find("Daily").GetComponent<UnityEngine.UI.Image>().color = disabledButton ;
        GameObject.Find("Subtitle").GetComponent<TMPro.TMP_Text>().text = "Daily";
    }
    private void Populate(List<UserDepth> spots)
    {
        if (spots == null || spots.Count == 0)
            spots = new List<UserDepth>();
        Debug.Log("Populating");
        for (int i = 0; i < spots.Count; i++)
        {
            GameObject spot = (GameObject)Instantiate(leaderboardElement, transform);
            LeaderboardElement lE = spot.GetComponent<LeaderboardElement>();
            lE.UpdateText(spots[i].UserName, spots[i].MaxDepth.ToString());
        }
    }

    private async void CallForSpots(bool isDaily)
    {
        if (isDaily)
        {
            dailySpots = await DatabaseCallUtility.FetchDailyLeaderboardSpots(10);
        }
        else
        {
            overallSpots = await DatabaseCallUtility.FetchLeaderboardSpots(10);
        }
    }

    private void EmptyTable()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }

    private IEnumerator WaitForSpots(bool isDaily)
    {
        CallForSpots(isDaily);
        Debug.Log("Starting Populating");
        yield return new WaitForSecondsRealtime(2);
        EmptyTable();
        Populate(isDaily ? dailySpots : overallSpots);
        yield break;
    }
}
