using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyObjectScript _enemyObjectScript;
    private GameObject _player;
    private bool _isFollowingPlayer;
    void Start()
    {
        try
        {
            _player = GameObject.FindFirstObjectByType<PlayerController>().gameObject;
            if (_enemyObjectScript == null)
                throw new System.Exception("No Enemy Scriptable Object available");
            if (_player == null)
                throw new System.Exception("No Player available");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if(_isFollowingPlayer)
        {
            FollowBehaviour();
        }
    }

    private void FollowBehaviour()
    {
        if (_player != null)
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            transform.position += direction * _enemyObjectScript.speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            Debug.Log("Player found");
            _isFollowingPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            Debug.Log("Player lost");
            _isFollowingPlayer = false;
        }
    }
}
