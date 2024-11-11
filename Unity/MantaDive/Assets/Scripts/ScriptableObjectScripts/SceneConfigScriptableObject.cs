using UnityEngine;

[CreateAssetMenu(fileName = "SceneConfigScriptableObject", menuName = "Scriptable Objects/SceneConfigScriptableObject")]
public class SceneConfigScriptableObject : ScriptableObject
{
    public string sceneName;
    public AudioClip backgroundMusic;
    public bool isLoadedAdditive;
}
