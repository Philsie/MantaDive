using UnityEngine;

public class ShopElement : MonoBehaviour
{
    public void OnClick()
    {
        ShopElementController controller = GameObject.FindFirstObjectByType<ShopElementController>();
        controller.ElementClicked(this);
    }
}
