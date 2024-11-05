using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int playerStamina = 100;
    public float playerSpeed = 0;
    public int startsCount = 0;
    public float magnetRadius = 0;
}
