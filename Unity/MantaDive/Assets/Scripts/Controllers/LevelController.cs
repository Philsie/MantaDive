using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private Material _backgroundMaterial;
    [SerializeField]
    private float _speed = 0.1f;
    [SerializeField]
    private float _speedModifier = 5f;
    private BoundriesController _boundriesController;
    private Vector2 _playAreaDimensions;
    [SerializeField]
    private float modifier = 1.7f;
    void Start()
    {
        GameObject.FindGameObjectWithTag("Background").gameObject.GetComponent<Image>().material = _backgroundMaterial;
        _boundriesController = FindFirstObjectByType<BoundriesController>()
            .GetComponent<BoundriesController>();
        _playAreaDimensions = _boundriesController.playerBoundries;
    }
    void Update()
    {
        float distance = _speed * Time.deltaTime;
        _backgroundMaterial.mainTextureOffset -= new Vector2 (0, distance);
        foreach (Transform child in transform) {
            child.position += new Vector3(0, distance * 1 * _speedModifier , 0);
            if(child.localPosition.y > _boundriesController.playerBoundries.y / modifier)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
