using Unity.VisualScripting;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    private static SessionManager Instance { get; set; }

    private int userID = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private static SessionManager GetInstance()
    {
        if (Instance == null)
        {
            GameObject singletonObject = new GameObject(nameof(SessionManager));
            Instance = singletonObject.AddComponent<SessionManager>();
            DontDestroyOnLoad(singletonObject);

            InitializeSession("dummy", "hunter2");
        }

        return Instance;
    }

    public static bool InitializeSession(string username, string password)
    {
        //TODO: Initialize session here after login flow is implemented
        //Dummy value is assigned for now
        GetInstance().userID = 0;

        return true;
    }

    public static int GetUserID()
    {
        return GetInstance().userID;
    }
}
