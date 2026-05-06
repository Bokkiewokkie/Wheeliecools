using UnityEngine;

public class ShopItemSpawner : MonoBehaviour
{
    public ShopItem ItemPrefab;
   
    void Start()
    {
        foreach(Liquid Drink in InventoryManager.Instance.Liquids)
        {
            ShopItem Item = Instantiate(ItemPrefab);
            Item.transform.SetParent(gameObject.transform, false);
            Item.OnSpawn(Drink.color);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
