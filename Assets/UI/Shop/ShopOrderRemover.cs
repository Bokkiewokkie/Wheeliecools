using UnityEngine;

public class ShopOrderRemover : MonoBehaviour
{
    public string liquidName; //The thing we're removing

    public void Trigger()
    {
        CheckoutManager.Instance.RemoveFromCart(liquidName);
        Destroy(gameObject);
    }

    public void Begone()
    {
        Destroy(gameObject);
    }

}
