using UnityEngine;

public class ShopItemSpawner : MonoBehaviour
{
    public ShopItem ItemPrefab;
   
    void Start()
    {
        int run = 0;
        foreach(Liquid Drink in InventoryManager.Instance.Liquids)
        {
            run++;
            if(run < 8){
                ShopItem Item = Instantiate(ItemPrefab);
                Item.transform.SetParent(gameObject.transform, false);
                Item.OnSpawn(Drink.color, Drink.liquidName, Drink.price);
            }
        }    
    }
}
