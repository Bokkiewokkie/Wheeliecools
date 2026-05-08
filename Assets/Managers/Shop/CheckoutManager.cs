using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class CheckoutManager : MonoBehaviour
{
    [SerializeField] private InventoryManager Inventory;
    [SerializeField] private TextMeshProUGUI Listing;
    [SerializeField] private TextMeshProUGUI TotalCost;
    [SerializeField] private TheWheel Wheel;
    [SerializeField] private ShopWheel WheelValue;
    private Dictionary<Liquid, int> Cart = new Dictionary<Liquid,int>(); //Liquid and how many liters we ordered
    public float PercentageFee = 10f;

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
        Liquid Drink = Inventory.NameToLiquid(DrinkName);
        if(Cart.TryGetValue(Drink, out int value)) //Check if it exists
        {
            Cart[Drink] += 1;
        }
        else
        {
            Cart.Add(Drink, 1);
        }
    }

    public void RemoveFromCart(string DrinkName)
    {
        Liquid Wawa = Inventory.NameToLiquid(DrinkName);
        Cart[Wawa] -= 1;
        if(Cart[Wawa] == 0) //Doesn't exist anymore so we don't need to list it
        {
            Cart.Remove(Wawa);
        }
        FormatCart();
    }

    public void FormatCart()
    {
        Listing.text = ""; //Clear it out first
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
        TotalCost.text += ((PercentageFee < 0) ? "Discount:" : "Fee:") + $" %{Math.Round(PercentageFee)}" + "\n";
        TotalCost.text += $"Total: ${CalculateTotalPrice(PercentageFee)}";
    }
    
    public void CalculateFee()
    {
        PercentageFee -= WheelValue.value;
    }

    private float CalculateItemPrice(Liquid Slurm)
    {
        float Amount;
        Amount = Cart[Slurm] * Slurm.price;
        return Amount;
    }

    //A negatiev fee is a discount
    private float CalculateTotalPrice(float Fee)
    {
        float Total = 0;
        foreach(var (Thang, amount) in Cart)
        {
            Total += CalculateItemPrice(Thang);
        }
        Total *= 1f + (Fee/100);
        return (float)Math.Round(Total, 2); //Just in case we get fractional cents
    }

    public void CompletePurchase()
    {
        if(Inventory.Funds > CalculateTotalPrice(PercentageFee)){
            foreach (var (Bottle, amount) in Cart)
            {
                float price = Bottle.price * amount * (1f + (PercentageFee/100));
                Purchase(Bottle, amount, price);
            }
            Cart.Clear(); //Don't forget to empty that out
            Wheel.Reset(); //After you complete your purchase, the wheel gets reset
            PercentageFee = 8f;
            WheelValue.Reset();
            FormatCart(); //And to show it
        }
    }

    public void Purchase(Liquid BoughtLiquid, int amount, float price)
    {
        float Funds = Inventory.Funds;
        Funds -= price;
        if(Funds > 0) //Oh you can actually afford it!
        {
            Inventory.AddLiquid(BoughtLiquid.liquidName, 100f * amount); //100cL, so a Liter of it (price is per liter)
            Inventory.Funds = (float)Math.Round(Funds, 2);
        }
    }
}
