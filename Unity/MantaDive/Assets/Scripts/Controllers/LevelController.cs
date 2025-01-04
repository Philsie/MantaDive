using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private Material _backgroundMaterial;
    [SerializeField]
    private float _speed = 0.1f;
    [SerializeField]
    private float _envitonmentItemsSpeedModifier = 5f;
    private BoundriesController _boundriesController;
    private Vector2 _playAreaDimensions;
    [SerializeField]
    private float _boundryModifier = 1.7f;
    public static bool isRunOngoing = false;
    private bool isPaused;

    [SerializeField]
    private TextMeshProUGUI currencyText;
    public TextMeshProUGUI CurrencyText => currencyText;
    [SerializeField]
    private TextMeshProUGUI premiumCurrencyText;
    public TextMeshProUGUI PremiumCurrencyText => premiumCurrencyText;
    [SerializeField]
    private TextMeshProUGUI staminaText;
    public TextMeshProUGUI StaminaText => staminaText;
    [SerializeField]
    private TextMeshProUGUI magnetText;
    public TextMeshProUGUI MagnetText => magnetText;

    void Start()
    {
        GameObject.FindGameObjectWithTag("Background").gameObject.GetComponent<Image>().material = _backgroundMaterial;
        _boundriesController = FindFirstObjectByType<BoundriesController>()
            .GetComponent<BoundriesController>();
        _playAreaDimensions = _boundriesController.playerBoundries;
        isRunOngoing = true;
    }
    void Update()
    {
        float distance = _speed * Time.deltaTime;
        _backgroundMaterial.mainTextureOffset -= new Vector2(0, distance);
        foreach (Transform child in transform)
        {
            child.position += new Vector3(0, distance * _envitonmentItemsSpeedModifier, 0);
            if (child.localPosition.y > _boundriesController.playerBoundries.y / _boundryModifier)
            {
                Destroy(child.gameObject);
            }
        }
        if (!isRunOngoing)
        {
            Debug.Log("Game ended");
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        Debug.Log("Game Paused");
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        Debug.Log("Game Unpaused");
    }
}
