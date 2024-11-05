using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SceneManagerController : MonoBehaviour
{
    public SceneConfigScriptableObject sceneConfigScriptableObject;
    public void LoadSceneFromConfig()
    {
        LoadScene(sceneConfigScriptableObject);
    }
    public void LoadScene(SceneConfigScriptableObject newSceneConfig)
    {
        if (newSceneConfig.backgroundMusic != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = newSceneConfig.backgroundMusic;
            audioSource.Play();
        }

        // Load the scene
        LoadSceneMode isAdditive = newSceneConfig.isLoadedAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
        SceneManager.LoadScene(newSceneConfig.sceneName, isAdditive);
    }
}
