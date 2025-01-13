using System;
using UnityEngine;

public class ProyectileController : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private int speed = 1;
    public int GetDamage()
    {
        return damage;
    }
    void Update()
    {
        transform.position = transform.position + Vector3.down * Time.deltaTime * speed;
        if (transform.position.y < -10)
            Destroy(gameObject);
    }
}
