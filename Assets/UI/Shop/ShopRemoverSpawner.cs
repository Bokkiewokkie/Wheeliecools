using UnityEngine;
using System.Collections.Generic;

public class ShopRemoverSpawner : MonoBehaviour
{
    [SerializeField] private ShopOrderRemover ItemPrefab;
    public List<ShopOrderRemover> Items = new List<ShopOrderRemover>();
    public void SpawnRemovalButton(string LiquidName)
    {
        ShopOrderRemover Item = Instantiate(ItemPrefab);
        Item.transform.SetParent(gameObject.transform, false);
        Item.liquidName = LiquidName;
        Item.owner = this; //Not our object, just the script
        Items.Add(Item);
    }
    
    public void Reset()
    {
        foreach (ShopOrderRemover Item in Items)
        {
            if(Item == null) continue; //Whatever
            Item?.Begone();
        }
    }
}
