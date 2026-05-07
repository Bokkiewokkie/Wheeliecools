using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class CheckoutManager : MonoBehaviour
{
    [SerializeField] private InventoryManager Inventory;
    [SerializeField] private TextMeshProUGUI Listing;
    [SerializeField] private TextMeshProUGUI TotalCost;
    private Dictionary<Liquid, int> Cart = new Dictionary<Liquid,int>(); //Liquid and how many liters we ordered
    public float PercentageFee;

	public static CheckoutManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			Debug.Log("CheckoutManager already exists");
		}
		else
		{
			Instance = this;
		}
	}

    void Start()
    {
        Inventory = InventoryManager.Instance;
    }

    public void AddToCart(string DrinkName)
    {
        Cart[Inventory.NameToLiquid(DrinkName)] += 1;
    }

    public void RemoveFromCart(string DrinkName)
    {
        Liquid Wawa = Inventory.NameToLiquid(DrinkName);
        Cart[Wawa] -= 1;
        if(Cart[Wawa] == 0) //Doesn't exist anymore so we don't need to list it
        {
            Cart.Remove(Wawa);
        }
    }

    public void FormatCart()
    {
        int loop = 0;
        foreach(var (Thang, amount) in Cart)
        {
            ++loop;
            Listing.text += Thang.liquidName + $" ${CalculateItemPrice(Thang)}\n";
            if(loop > 10)
            {
                Listing.text += "...";
                break; //We just don't have space for more
            }
        }
        TotalCost.text = $"Subtotal: ${CalculateTotalPrice(0)}\n";
        TotalCost.text = $"Fee: %{Math.Round(PercentageFee)}" + "\n";
        TotalCost.text = $"Total: ${CalculateTotalPrice(PercentageFee)}";
    }

    public float CalculateItemPrice(Liquid Slurm)
    {
        float Amount;
        Amount = Cart[Slurm] * Slurm.price;
        return Amount;
    }

    //A negatiev fee is a discount
    public float CalculateTotalPrice(float Fee)
    {
        float Total = 0;
        foreach(var (Thang, amount) in Cart)
        {
            Total += CalculateItemPrice(Thang);
        }
        Total *= 1f + Fee;
        return Total;
    }

    public void Purchase(Liquid BoughtLiquid)
    {
        float Funds = Inventory.Funds;
        Funds -= BoughtLiquid.price;
        if(Funds > 0) //Oh you can actually afford it!
        {
            Inventory.AddLiquid(BoughtLiquid.liquidName, 100f); //100cL, so a Liter of it (price is per liter)
            Inventory.Funds = (float)Math.Round(Funds, 2);
        }
    }
}
