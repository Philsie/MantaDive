using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopElement : MonoBehaviour
{
    public int Id;
    public bool disabledPopup = false;

    public void openConfirmPopup()
    {
        if(disabledPopup == false){
            Transform confirmPopupTransform = transform.Find("ShopElementConfirmPopup");
            Transform ShopElementNormalTransform = transform.Find("ShopElementNormal");
            confirmPopupTransform.gameObject.SetActive(true);
            ShopElementNormalTransform.gameObject.SetActive(false);
        }
    }

    public void closeConfirmPopup()
    {
        Transform confirmPopupTransform = transform.Find("ShopElementConfirmPopup");
        Transform ShopElementNormalTransform = transform.Find("ShopElementNormal");
        confirmPopupTransform.gameObject.SetActive(false);
        ShopElementNormalTransform.gameObject.SetActive(true);
    }


    public void ConfirmPurchase()
    {
        DatabaseCallUtility.UnlockShopItemForUser(SessionManager.GetUserID(),this.Id);
        //Debug.Log($"{SessionManager.GetUserID()},{this.Id}");


        // Get the currently active scene
        Scene activeScene = SceneManager.GetActiveScene();

        // Reload the active scene
        SceneManager.LoadScene(activeScene.name);
    }
}
