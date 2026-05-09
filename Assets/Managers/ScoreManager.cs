using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private Customer please;
    [SerializeField] private Canvas HealthHUD;
    [SerializeField] private SellSpot Spot;
    [SerializeField] private TextMeshProUGUI HealthBar;

    [SerializeField] private List<GlassSpot> Allat = new List<GlassSpot>();
    public int failedrequests = 0;
    public int completedrequests = 0;

	// Singleton design pattern, only 1 ScoreManager can exist at a time.
	public static ScoreManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			Debug.Log("ScoreManager already exists");
		}
		else
		{
			Instance = this;
		}
        Spot.RequestFulfilled.AddListener(RequestFullfilled);
        please.OnRequestExpired.AddListener(RequestFailed);
        HealthBar.text = "Customer will leave <u>after " + (3-failedrequests).ToString() + " more wrong orders</u>";
	}


    private void OnDestroy()
    {
        Spot.RequestFulfilled.RemoveListener(RequestFullfilled);
        please.OnRequestExpired.RemoveListener(RequestFailed);
    }

    public void RequestFailed(Request request)
    {
        ++failedrequests;
        if(failedrequests > 2)
        {
            HealthHUD.enabled = false; //Begone
            SceneLoader.Instance.LoadScene(); //Sends us right to the endscreen
        }
        HealthBar.text = "Customer will leave <u>after " + (3-failedrequests).ToString() + " more expired orders</u>";
    }

    public void RequestFullfilled(Request request)
    {
        ++completedrequests;
        foreach(GlassSpot spot in Allat) //Replace that thang
        {
		    spot.SpawnGlass();
        }
    }

}
