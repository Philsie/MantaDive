using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState")]
public class GameState : ScriptableObject
{
    public bool isGamePaused = false;
    public bool isGameOver = false;
    public bool isUpgrading = false;
    public string currentGamePhase = "Main";  // The current phase (e.g., "Main", "Upgrade", "GameOver")
}
