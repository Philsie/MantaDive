using UnityEngine;
using UnityEngine.EventSystems;

public class InstantiateEventSystem : MonoBehaviour
{
    void Start()
    {
        // Check if an EventSystem already exists in any of the loaded scenes
        EventSystem eventSystem = Object.FindFirstObjectByType<EventSystem>();

        if (eventSystem == null)
        {
            // No EventSystem found, create one
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
            Debug.Log("EventSystem created in the additively loaded scene.");
        }
        else
        {
            Debug.Log("EventSystem already exists in the first scene.");
        }
    }
}
