using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PageTracker;

    private int PageNumber = 1;
    private int PageAmount = 1;

    
	public static ShopManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			Debug.Log("ShopManager already exists");
		}
		else
		{
			Instance = this;
		}
	}


    // Always changes only one page at a time
    public void ChangePage(bool forward)
    {
        if (forward)
        {
            PageNumber = Math.Min(++PageNumber,PageAmount);
        }
        else
        {
            PageNumber = Math.Max(--PageNumber, 1);
        }
        PageTracker.text = PageNumber.ToString();
    }

    public void Purchase(string Drinkname)
    {
        CheckoutManager.Instance.AddToCart(Drinkname);
    }

/*
    public void Purchase(string DrinkName)
    {
        Liquid BoughtLiquid = Inventory.NameToLiquid(DrinkName);
        float Funds = Inventory.Funds;
        Funds -= BoughtLiquid.price;
        if(Funds > 0) //Oh you can actually afford it!
        {
            Inventory.AddLiquid(DrinkName, 100f); //100cL, so a Liter of it (price is per liter)
            Inventory.Funds = (float)Math.Round(Funds, 2);
        }
    }
*/
}

