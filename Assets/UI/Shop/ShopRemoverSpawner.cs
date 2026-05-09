using UnityEngine;

public class ShopRemoverSpawner : MonoBehaviour
{
    [SerializeField] private ShopOrderRemover ItemPrefab;
    private List<ShopOrderRemover> Items;
    public void SpawnRemovalButton(string LiquidName)
    {
        ShopOrderRemover Item = Instantiate(ItemPrefab);
        Item.transform.SetParent(gameObject.transform, false);
        Item.liquidName = LiquidName;
        Items.Add(Item);
    }
    
    public void Reset()
    {
        foreach (ShopOrderRemover Item in Items)
        {
            Item.Begone();
        }
    }
}
