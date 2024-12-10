using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoundriesController : MonoBehaviour
{
    public Camera camera;
    public Vector2 playerBoundries { get; private set; }

    private void Start()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        playerBoundries = CalculatePlayerBoundries(camera, 0);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>() != null)
        {
            Debug.Log("Enemy left game area");
            Destroy(collision.gameObject);
        }
    }

    private Vector2 CalculatePlayerBoundries(Camera camera, float depth)
    {
        float distance = Mathf.Abs(camera.transform.position.z - depth);
        float height = 2.0f * Mathf.Tan(camera.fieldOfView / 2.0f * Mathf.Deg2Rad) * distance;
        float width = height * camera.aspect;
        Debug.Log(new Vector2 (width, height));
        return new Vector2(width, height);
    }
}
    