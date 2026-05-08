using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ShopNav : MonoBehaviour
{
    [SerializeField] private Canvas ShopWindow;
    [SerializeField] private Canvas CheckoutWindow;
    [SerializeField] private Canvas NavigationButtons;
    [SerializeField] private Button ShopButton;
    [SerializeField] private Button CheckoutButton;
    [SerializeField] private TextMeshProUGUI FundTracker;
    [SerializeField] private TheWheel Wheel;
    public UnityEvent ExitShopEvent;

    void Start() //Don't start with the shop open lol
    {
        Wheel.gameObject.SetActive(false);
        NavigationButtons.enabled = false;
        ShopWindow.enabled = false;
        CheckoutWindow.enabled = false;
    }

    void Update()
    {
        FundTracker.text = "Funds: $" + InventoryManager.Instance.Funds.ToString();
    }

    public void ShopLoad() //First time opening the shop
    {
        AudioManager.Instance.InterruptWith(3,true);
        NavigationButtons.enabled = true;
        EnterShop();
    }

    public void EnterShop()
    {
        Wheel.gameObject.SetActive(false);
        CheckoutWindow.enabled = false;
        ShopWindow.enabled = true;
        ShopButton.interactable = false;
        CheckoutButton.interactable = true;
    }

    public void EnterCheckout()
    {
        Wheel.gameObject.SetActive(true);
        ShopWindow.enabled = false;
        CheckoutWindow.enabled = true;
        CheckoutButton.interactable = false;
        ShopButton.interactable = true;
        CheckoutManager.Instance.FormatCart(); //Update the cart display
    }

    public void ExitShop()
    {
        Wheel.gameObject.SetActive(false);
        NavigationButtons.enabled = false;
        ShopWindow.enabled = false;
        CheckoutWindow.enabled = false;
        AudioManager.Instance.ResumeTrack(false);
        ExitShopEvent.Invoke();
    }
}
