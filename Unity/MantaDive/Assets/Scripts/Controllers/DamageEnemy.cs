using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    private int hp = 1;

    private void Start()
    {
        hp = transform.parent.GetComponent<EnemyController>()._enemyObjectScript.hitPoints;
    }
    void Update()
    {

        if (hp <= 0)
        {
            MetaDataManager.Instance.IncrementEnemiesHit();
            Destroy(transform.parent.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ProyectileController>())
        {
            hp -= collision.gameObject.GetComponent<ProyectileController>().GetDamage();
            Destroy(collision.gameObject);
        }
    }
}
