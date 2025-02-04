using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private Material _backgroundMaterial;
    [SerializeField]
    private float _envitonmentItemsSpeedModifier = 1f;
    [SerializeField]
    private float _backgroundSpeedModifier = .01f;
    private BoundriesController _boundriesController;
    private Vector2 _playAreaDimensions;
    [SerializeField]
    private float _boundryModifier = 1.7f;

    [SerializeField]
    private GameObject premiumPrefab;
    [SerializeField]
    private GameObject currencyPrefab;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private GameObject[] upgradePrefabs;
    [SerializeField]
    private int xSpawnRange = 3;
    [SerializeField]
    private int spawnInterval = 3;
    private float spawnYPosition = -10;

    void Start()
    {
        GameObject.FindGameObjectWithTag("Background").gameObject.GetComponent<Image>().material = _backgroundMaterial;
        _boundriesController = FindFirstObjectByType<BoundriesController>()
            .GetComponent<BoundriesController>();
        _playAreaDimensions = _boundriesController.playerBoundries;
        StartCoroutine(SpawnPeriodically());
    }
    void Update()
    {
        MoveEnvironmentElemtnsUp();
    }

    private void MoveEnvironmentElemtnsUp()
    {
        float distance = PlayerStatsManager.GetPlayerCurrentSpeed() * Time.deltaTime * _envitonmentItemsSpeedModifier;
        _backgroundMaterial.mainTextureOffset -= new Vector2(0, distance * _backgroundSpeedModifier);
        foreach (Transform child in transform)
        {
            child.position +=
                new Vector3(0, distance, 0);
            if (child.localPosition.y > _boundriesController.playerBoundries.y / _boundryModifier)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void SpawnElement()
    {
        int nextNumber = RunManager.GetNextRandomNumber();
        switch (nextNumber)
        {
            case int i when i < 520:
                int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                Spawn(enemyPrefabs[enemyIndex]);
                break;
            case int i when i < 800:
                Spawn(currencyPrefab);
                break;
            case int i when i < 810:
                Spawn(premiumPrefab);
                break;
            case int i when i < 1000:
                int upgradeIndex = Random.Range(0, upgradePrefabs.Length);
                Spawn(upgradePrefabs[upgradeIndex]);
                break;
            default:
                break;
        }
    }

    private void Spawn(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Prefab is not assigned!");
            return;
        }

        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        Vector3 spawnPosition = new Vector3(randomX, spawnYPosition, 0f);
        Instantiate(prefab, spawnPosition, Quaternion.identity, gameObject.transform);
    }

    private IEnumerator SpawnPeriodically()
    {
        yield return new WaitUntil(() => RunManager.IsRunOngoing());
        while (RunManager.IsRunOngoing())
        {
            yield return new WaitUntil(() => !RunManager.IsGamePaused());
            yield return new WaitForSeconds(Random.Range((int) 1, (int) 4));
            SpawnElement();
        }
    }

}
