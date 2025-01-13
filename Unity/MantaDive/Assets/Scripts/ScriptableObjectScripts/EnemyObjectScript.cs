using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObjectScript", menuName = "Scriptable Objects/EnemyObjectScript")]
public class EnemyObjectScript : ScriptableObject
{
    public string enemyName;
    public Texture2D material;
    public bool followsPlayer;
    public float speed;
    public int hitPoints;
    public float damage;
}
