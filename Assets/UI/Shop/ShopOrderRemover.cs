using UnityEngine;

public class ShopOrderRemover : MonoBehaviour
{
    public string liquidName; //The thing we're removing
    public ShopRemoverSpawner owner;

    public void Trigger()
    {
        CheckoutManager.Instance.RemoveFromCart(liquidName);
        owner.Items?.Remove(this); //Delete our ref from the list
        Destroy(gameObject);
    }

    public void Begone()
    {
        Destroy(gameObject);
    }

}
