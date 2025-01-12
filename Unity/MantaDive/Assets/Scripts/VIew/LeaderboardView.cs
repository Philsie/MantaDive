using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField]
    private GameObject leaderboardElement;
    private List<UserDepth> overallSpots = new List<UserDepth>();
    private List<UserDepth> dailySpots = new List<UserDepth>();
    private bool isDailyShown = false;
    private bool isOverallShown = false;

    public void PopulateOverall()
    {
        if (isOverallShown) return;
        StartCoroutine(WaitForSpots(false));
        isDailyShown = false;
        isOverallShown = true;
    }
    public void PopulateDailyl()
    {
        if (isDailyShown) return;
        StartCoroutine(WaitForSpots(true));
        isDailyShown = true;
        isOverallShown = false;
    }
    private void Populate(List<UserDepth> spots)
    {
        if (spots == null || spots.Count == 0) return; 
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
        EmptyTable();
        CallForSpots(isDaily);
        yield return new WaitForSeconds(2);
        Populate(isDaily ? dailySpots : overallSpots);
        yield break;
    }
}
