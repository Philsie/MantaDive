using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class RunController : MonoBehaviour
{
    private float staminaLossSpeed = 0.5f;
    [SerializeField]
    private SceneManagerController sceneManager;
    [SerializeField]
    private SceneConfigScriptableObject endScene;
    [SerializeField]
    private SceneConfigScriptableObject pauseScene;
    [SerializeField]
    private SceneConfigScriptableObject levelScene;
    private static bool hasDbBeenUpdated = false;

    async void Start()
    {
        hasDbBeenUpdated = false;
        if (RunManager.IsDailyRun())
        {
            int seed = await DatabaseCallUtility.FetchDailySeed();
            RunManager.SetSeed(seed);
        } else
        {
            int randomSeed = Random.Range(0, 1000000);
            RunManager.SetSeed(randomSeed);
        }
        StartCoroutine(BeginRun());
        StartCoroutine(ReduceStamina());
        StartCoroutine(IncreaseDepth());
    }

    private void OnEnable()
    {
        RunManager.GetInstance().OnRunStateChange += EndRun;
        SceneManager.sceneUnloaded += OnPauseSceneUnloaded;
        SceneManager.sceneUnloaded += ResetValues;
        PlayerStatsManager.ChangePlayerMagnetStrengthByAmount(RunManager.GetExtraMagnet());
        PlayerStatsManager.ChangePlayerCurrentStaminaByAmount(RunManager.GetExtraStamina());
        PlayerStatsManager.RefreshLevelUI();
    }
    private void OnDisable()
    {
        RunManager.GetInstance().OnRunStateChange -= EndRun;
    }

    private void ResetValues(Scene scene)
    {
        if(scene.name.Equals(levelScene.name))
        {
            PlayerStatsManager.SetPlayerDepth(0);
            PlayerStatsManager.SetPlayerCurrentStamina(PlayerStatsManager.GetPlayerMaxStamina());
            PlayerStatsManager.SetPlayerMagnetStrength(RunManager.GetBaseMagnet());
            RunManager.SetExtraStamina(0);
            RunManager.SetExtraMagnet(0);
        }
    }

    private void OnPauseSceneUnloaded(Scene scene)
    {
        if (scene.name.Equals(pauseScene.name))
        {
            UnpauseGame();
        }
    }

    private IEnumerator BeginRun()
    {
        PlayerStatsManager.RefreshLevelUI();
        yield return new WaitForSecondsRealtime(3);
        UnpauseGame();
        RunManager.SetIsRunOngoing(true);
    }

    public void EndRun()
    {
        if (!RunManager.IsRunOngoing())
        {
            PauseGame();
            RunManager.SetIsGamePaused(true);
            sceneManager.LoadScene(endScene);
            UpdateDB();
        }
    }

    private async void UpdateDB()
    {
        if (hasDbBeenUpdated) return;
        hasDbBeenUpdated = true;

        Debug.Log("Depth reached: " + PlayerStatsManager.GetPlayerDepth());
        Debug.Log("Current Coins: " + CollectiblesManager.GetPrimaryCurrency());
        Debug.Log("Current Premium: " + CollectiblesManager.GetPremiumCurrency());
        //Update Database
        int userID = SessionManager.GetUserID();
        User user = await DatabaseCallUtility.FetchUserData(userID);
        await DatabaseCallUtility.UpdateUserPrimaryCurrency(userID, CollectiblesManager.GetPrimaryCurrency());
        await DatabaseCallUtility.UpdateUserPremiumCurrency(userID, CollectiblesManager.GetPremiumCurrency());
        if (RunManager.IsDailyRun())
        {
            if (user.DailyDepth < PlayerStatsManager.GetPlayerDepth())
                await DatabaseCallUtility
                    .UpdateUserDailyDepth(userID, PlayerStatsManager.GetPlayerDepth());
        }
        else
        {
            if (user.MaxDepth < PlayerStatsManager.GetPlayerDepth())
                await DatabaseCallUtility
                    .UpdateUserMaxDepth(userID, PlayerStatsManager.GetPlayerDepth());
        }
    }
    private IEnumerator ReduceStamina()
    {
        yield return new WaitUntil(() => RunManager.IsRunOngoing());
        while (RunManager.IsRunOngoing())
        {
            yield return new WaitUntil(() => !RunManager.IsGamePaused());
            yield return new WaitForSeconds(staminaLossSpeed);
            PlayerStatsManager.ChangePlayerCurrentStaminaByAmount(-1);

            if (PlayerStatsManager.GetPlayerCurrentStamina() <= 0)
            {
                RunManager.SetIsRunOngoing(false);
                yield break;
            }
        }
    }

    private IEnumerator IncreaseDepth()
    {
        yield return new WaitUntil(() => RunManager.IsRunOngoing());
        while (RunManager.IsRunOngoing())
        {
            yield return new WaitUntil(() => !RunManager.IsGamePaused());
            yield return new WaitForSeconds(1 / PlayerStatsManager.GetPlayerCurrentSpeed());
            PlayerStatsManager.ChangePlayerDepthByValue(1);

            if (!RunManager.IsRunOngoing())
            {
                yield break;
            }
        }
    }

    public static void PauseGame()
    {
        RunManager.SetIsGamePaused(true);
        Time.timeScale = 0;
    }

    public static void UnpauseGame()
    {
        RunManager.SetIsGamePaused(false);
        Time.timeScale = 1;
    }
}
