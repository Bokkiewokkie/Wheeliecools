using System;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public float Funds = 0; //Our cash //TODO: pulled from the inventory manager
    [SerializeField] private TextMeshProUGUI PageTracker;
    [SerializeField] private TextMeshProUGUI FundTracker;

    private int PageNumber = 1;
    private int PageAmount = 4;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FundTracker.text = "Funds $" + Funds.ToString();
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
}
