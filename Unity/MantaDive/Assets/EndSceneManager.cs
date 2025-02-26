using TMPro;
using UnityEngine;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text time;
    [SerializeField]
    private string textTime = "0"; 
    [SerializeField]
    private TMP_Text shots;
    [SerializeField]
    private string textShots = "0"; 
    [SerializeField]
    private TMP_Text hits;
    [SerializeField]
    private string textHits = "0";
    [SerializeField]
    private TMP_Text shells;
    [SerializeField]
    private string textShells = "0";


    private void Start()
    {
        MetaDataManager metaDataManager = MetaDataManager.Instance;
        textTime = metaDataManager.GetCurrentTime().ToString("0.00") + "s";
        textShots = metaDataManager.GetShotsFired().ToString();
        textShells = metaDataManager.GetCoinsCollected().ToString();
        textHits = metaDataManager.GetEnemiesHit().ToString();

        time.text = textTime;
        shells.text = textShells;
        shots.text = textShots;
        hits.text = textHits;
    }
}