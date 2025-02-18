using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MetaDataManager : MonoBehaviour
{
    private static MetaDataManager _instance;

    private float _timeElapsed;
    private int _shotsFired;
    private int _enemiesHit;
    private int _coinsCollected;

    public static MetaDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MetaDataManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("MetaDataManager");
                    _instance = go.AddComponent<MetaDataManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Public methods for time
    public float GetCurrentTime() => _timeElapsed;
    public void ResetTime() => _timeElapsed = 0f;
    public void AddTime(float amount) => _timeElapsed += amount;

    // Public methods for shots
    public int GetShotsFired() => _shotsFired;
    public void SetShotsFired(int value) => _shotsFired = value;
    public void IncrementShots(int amount = 1) => _shotsFired += amount;

    // Public methods for enemies hit
    public int GetEnemiesHit() => _enemiesHit;
    public void SetEnemiesHit(int value) => _enemiesHit = value;
    public void IncrementEnemiesHit(int amount = 1) => _enemiesHit += amount;

    // Public methods for coins collected
    public int GetCoinsCollected() => _coinsCollected;
    public void SetCoinsCollected(int value) => _coinsCollected = value;
    public void IncrementCoinsCollected(int amount = 1) => _coinsCollected += amount;

    // Reset all data for new level
    public void ResetAllData()
    {
        _timeElapsed = 0f;
        _shotsFired = 0;
        _enemiesHit = 0;
        _coinsCollected = 0;
    }
    public async void SendDataToAPI()
    {
        bool success = await DatabaseCallUtility.PostLevelMetaData(
            _timeElapsed,
            _shotsFired,
            _enemiesHit,
            _coinsCollected
        );

        if (success)
        {
            Debug.Log("Level metadata sent successfully!");
        }
        else
        {
            Debug.LogError("Failed to send level metadata.");
        }
    }
}