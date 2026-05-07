using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Liquid[] Liquids;
    private Dictionary<string, float> LiquidsInventory = new Dictionary<string,float>(); //Type of liquid and an amount in cL
    public float Funds = 10; //Our money in dollars

	public static InventoryManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			Debug.Log("InventoryManager already exists");
		}
		else
		{
			Instance = this;
		}
	}

    void Start()
    {
        foreach(Liquid Bubbles in Liquids){
            LiquidsInventory.Add(Bubbles.liquidName, 20f); //We get 20 cL of every drink to start
        }
    }

    public Liquid NameToLiquid(string LiquidName)
    {
        foreach(Liquid Found in Liquids){
            if(Found.liquidName != LiquidName)
            {
                continue;
            }
            return Found;
        }
        return null; //I dunno what you were looking for but it's not here
    }

    public float AmountOfLiquid(string LiquidName)
    {
        return LiquidsInventory[LiquidName];
    }

    public bool TakeLiquid(string LiquidName, float amount)
    {
        float Available = LiquidsInventory[LiquidName];
        if(Available - amount > 0)
        {
            LiquidsInventory[LiquidName] -= amount;
            return true;
        }
        return false; //You can't take this out if you don't have this much
    }

    public void AddLiquid(string LiquidName, float amount)
    {
        LiquidsInventory[LiquidName] += amount;
    }
}
