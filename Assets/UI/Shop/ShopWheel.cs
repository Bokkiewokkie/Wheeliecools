using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopWheel : MonoBehaviour
{
    // THIS IS HOW YOU LEARN TO WHEEL
    [SerializeField] private TheWheel theWheel;
    [SerializeField] private TextMeshProUGUI Mood;
    public float value;

    private void Awake()
    {
        theWheel.newDirChosen.AddListener(GUpdateOutput);
        theWheel.selectionMade.AddListener(GetResult);
    }

    private void OnDestroy()
    {
        theWheel.newDirChosen.RemoveListener(GUpdateOutput);
        theWheel.selectionMade.RemoveListener(GetResult);
    }

    public void GUpdateOutput(WheelPayload payload)
    {
        string outputString = string.Empty;
        
        switch (payload.BaseValue)
        {
            case -2:
                outputString += "The shopkeeper looks angry";
                break;
            case -1:
                outputString += "The shopkeeper looks upset";
                break;
            case 1:
                outputString += "The shopkeeper looks pleased";
                break;
            case 2:
                outputString += "The shopkeeper looks very agreeable";
                break;
            default:
                outputString += $"The shopkeeper tells you about a secret code starting with {payload.BaseValue}...";
                break;
        }
        
        Mood.text = outputString;
        CheckoutManager.Instance.FormatCart();
    }

    public void GetResult(WheelPayload payload)
    {
        value += payload.BaseValue * (payload.SliceValue/2);
        CheckoutManager.Instance.CalculateFee();
    }

    public void Reset()
    {
        value = 0;
    }
}
