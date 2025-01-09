using UnityEngine;
using System.Collections;

public class RunController : MonoBehaviour
{

    private float staminaLossSpeed = 0.5f;

    async void Start()
    {
        if(RunManager.IsDailyRun())
        {
            int seed = await DatabaseCallUtility.FetchDailySeed();
            RunManager.SetSeed(seed);
        } else
        {
            int randomSeed = Random.Range(0, 1000000);
            RunManager.SetSeed(randomSeed);
        }
        RunManager.SetIsRunOngoing(true);
        StartCoroutine(ReduceStamina());
    }

    private IEnumerator ReduceStamina()
    {
        Debug.Log("Reducing stamina");
        yield return new WaitUntil(() => RunManager.IsRunOngoing());
        while (RunManager.IsRunOngoing())
        {
            yield return new WaitUntil(() => RunManager.IsGamePaused());
            yield return new WaitForSeconds(staminaLossSpeed);
            PlayerStatsManager.ChangePlayerCurrentStaminaByAmount(-1);

            if (PlayerStatsManager.GetPlayerCurrentStamina() <= 0)
            {
                RunManager.SetIsRunOngoing(false);
                yield break;
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        RunManager.SetIsGamePaused(true);
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
        RunManager.SetIsGamePaused(false);
    }
}
