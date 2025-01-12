using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopElement : MonoBehaviour
{
    public int Id;


    public void OnClick()
    {
        DatabaseCallUtility.UnlockShopItemForUser(SessionManager.GetUserID(),this.Id);
        //Debug.Log($"{SessionManager.GetUserID()},{this.Id}");
        // Get the currently active scene
        Scene activeScene = SceneManager.GetActiveScene();

        // Reload the active scene
        SceneManager.LoadScene(activeScene.name);
    }
}
