using UnityEngine;

public class MagnetController : MonoBehaviour
{
    [SerializeField] float speed = 1;
    private void Update()
    {
        GetComponent<CircleCollider2D>().radius = PlayerStatsManager.GetPlayerMagnetStrength();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Collectible")
        {
            AttractCollectibles(collision.transform);
        }
    }
    private void AttractCollectibles(Transform collectibleTransform)
    {
        Vector3 direction = (transform.parent.transform.position - collectibleTransform.position).normalized;
        collectibleTransform.position += direction * speed * Time.deltaTime;

    }
}
