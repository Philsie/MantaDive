using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckCamera : MonoBehaviour
{
    void Start()
    {
        // Check if a camera already exists in any of the loaded scenes
        Camera mainCamera = Object.FindFirstObjectByType<Camera>();

        if (mainCamera == null)
        {
            GameObject cameraObject = new GameObject("MainCamera");
            Camera cameraComponent = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();

            cameraObject.tag = "MainCamera";

            Debug.Log("Main Camera created in the additively loaded scene.");
        }
        else
        {
            Debug.Log("Main Camera already exists in the first scene.");
        }
    }
}