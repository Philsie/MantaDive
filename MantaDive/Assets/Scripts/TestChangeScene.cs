using UnityEngine;

public class TestChangeScene : MonoBehaviour
{
    public SceneManagerController SceneManagerController;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            SceneManagerController.LoadSceneFromConfig();
        }
    }
}
