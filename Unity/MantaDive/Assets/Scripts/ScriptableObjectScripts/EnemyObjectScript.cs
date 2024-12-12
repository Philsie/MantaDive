using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObjectScript", menuName = "Scriptable Objects/EnemyObjectScript")]
public class EnemyObjectScript : ScriptableObject
{
    public string name;
    public Texture2D material;
    public bool followsPlayer;
    public float speed;
    public float hitPoints;
    public float damage;
}
