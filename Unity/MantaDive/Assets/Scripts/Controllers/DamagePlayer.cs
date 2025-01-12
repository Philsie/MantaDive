using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerStatsManager.ChangePlayerCurrentStaminaByAmount(-damage);
        }
    }
}
