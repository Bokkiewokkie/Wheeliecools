using UnityEngine;

[RequireComponent(typeof(GlassSpot))]
public class SellSpot : MonoBehaviour
{
	public float perfectMultiplier = 2f;
	[Range(0f, 1f)]
	public float minimumMultiplier = 0.1f;
	[Range(0f, 100f)]
	public float perfectionMargin = 10f;


	private GlassSpot glassSpot;
	private bool wasEmpty = true;

	private void Awake()
	{
		glassSpot = GetComponent<GlassSpot>();
	}

	private void Update()
	{
		bool isEmpty = glassSpot.IsEmpty;

		if (wasEmpty && !isEmpty)
			TrySell();

		wasEmpty = isEmpty;
	}

	private void TrySell()
	{
		Glass glass = glassSpot.CurrentGlass;
		if (glass == null) return;

		var requests = RequestManager.Instance?.Requests;
		if (requests == null || requests.Count == 0)
		{
			Debug.Log("SellSpot: No pending requests - glass returned.");
			return;
		}

		int bestIndex = -1;
		Glass.RecipeScore bestScore = default;
		float bestTotal = float.NegativeInfinity;

		for (int i = 0; i < requests.Count; i++)
		{
			Request req = requests[i];
			if (req == null || req.recipe == null) continue;

			Glass.RecipeScore score = glass.CheckRecipe(req);

			if (score.total > bestTotal)
			{
				bestIndex = i;
				bestScore = score;
				bestTotal = score.total;
			}
		}

		if (bestIndex == -1)
		{
			Debug.LogWarning("SellSpot: All requests were null/invalid - glass returned.");
			return;
		}

		Request matched = requests[bestIndex];
		float payout = CalculatePayout(glass, bestScore);

		InventoryManager.Instance.Funds += payout;

		Debug.Log($"SellSpot: Matched '{matched.recipe.recipeName}' " +
				  $"(score {bestScore.total:0.0}/100)  +${payout:0.00} " +
				  $"[Ingredients: {bestScore.ingredientScore:0.0}  " +
				  $"Purity: {bestScore.purityScore:0.0}  " +
				  $"Fill: {bestScore.fillScore:0.0}]");

		requests.RemoveAt(bestIndex);
		glassSpot.TakeGlass();
		Destroy(glass.gameObject);
	}

	private float CalculatePayout(Glass glass, Glass.RecipeScore score)
	{
		float baseValue = 0f;

		foreach (var kvp in glass.LiquidAmounts)
		{
			Liquid liquid = kvp.Key;
			float proportion = kvp.Value;

			baseValue += GetLiquidValue(liquid) * proportion;
		}

		float threshold = 100f - Mathf.Clamp(perfectionMargin, 0f, 100f);
		float t = Mathf.Clamp01(score.total / threshold);

		float multiplier = Mathf.Lerp(minimumMultiplier, perfectMultiplier, t);

		return baseValue * multiplier;
	}

	private static float GetLiquidValue(Liquid liquid)
	{
		if (liquid == null) return 0f;

		return liquid.price;
	}
}