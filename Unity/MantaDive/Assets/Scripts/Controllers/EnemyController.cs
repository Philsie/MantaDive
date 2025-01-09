using System;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyObjectScript _enemyObjectScript;
    private GameObject _player;
    private bool _isPlayerFound = false;
    [SerializeField]
    private float _angleOffset = -90;
    [SerializeField]
    private float _speedIncreaseFactor = 1.5f;
    [SerializeField]
    private float _speedDecreaseFactor = 0.5f;
    private float _speedModifier;
    private float _rotationSpeed = 5f;
    private Vector3 _endTarget = new Vector3(0,100,0);

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
        bool isEnemyUnderPlayer = EnemyIsUnderPlayer();
        if (_isPlayerFound && isEnemyUnderPlayer)
        {
            FollowPlayer();
        }
        else if (!isEnemyUnderPlayer)
        {
            _isPlayerFound = false;
            DecreaseSpeed();
            MoveTowardsTarget(_endTarget);
        }
        else
        {
            MoveTowardsTarget(_endTarget);
        }
    }

    private void FollowPlayer()
    {
        if (_player != null)
        {
            MoveTowardsTarget(_player.transform.position);
        }
    }
    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + _angleOffset;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        transform.position += direction * _enemyObjectScript.speed * _speedModifier * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerIsFound();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerIsLost();
        }
    }

    private void PlayerIsFound()
    {
        _isPlayerFound = true;
        IncreaseSpeed();
    }
    private void PlayerIsLost()
    {
        _isPlayerFound = false;
        DecreaseSpeed();
    }
    private bool EnemyIsUnderPlayer()
    {
        return transform.position.y < _player.transform.position.y;
    }
    private void DecreaseSpeed()
    {
        _speedModifier = _speedDecreaseFactor;
    }

    private void IncreaseSpeed()
    {
        _speedModifier = _speedIncreaseFactor;
    }

}
