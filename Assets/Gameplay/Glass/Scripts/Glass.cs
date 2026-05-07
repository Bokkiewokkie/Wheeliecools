using UnityEngine;
using System.Collections.Generic;

public class Glass : MonoBehaviour
{
	// Thing that fills with liquid! stores how much liquid there is and fills with a mixed colour

	public Transform fillSquare;

	public int glassType = 0;

	[Header("Fill Scale Bounds")]
	public float minScaleY = 0.01f;
	public float maxScaleY = 1f;

	public float fillDuration = 3f;

	public struct IngredientScore
	{
		public string liquidName;
		public float targetAmount;
		public float actualAmount;
		public float score;            // 0-100
	}

	public struct RecipeScore
	{
		public float ingredientScore;
		public float purityScore;
		public float fillScore;
		public float total;

		public List<IngredientScore> ingredients;
	}

	private float fillAmount = 0f;
	public Color mixedColor = Color.white;
	public float totalFillAdded = 0f;

	private Dictionary<Liquid, float> liquidAmounts = new Dictionary<Liquid, float>();

	private float fillBottomLocalY;

	private Renderer fillRenderer;
	private MaterialPropertyBlock propBlock;

	public float FillAmount => fillAmount;
	public Color MixedColor => mixedColor;
	public bool IsFull => fillAmount >= 1f;
	public bool IsEmpty => fillAmount <= 0f;
	public float FillDelta => Time.deltaTime / Mathf.Max(fillDuration, 0.01f);

	public IReadOnlyDictionary<Liquid, float> LiquidAmounts => liquidAmounts;

	void Awake()
	{
		fillRenderer = fillSquare.GetComponent<Renderer>();
		propBlock = new MaterialPropertyBlock();
		fillBottomLocalY = fillSquare.localPosition.y - fillSquare.localScale.y * 0.5f;
		ApplyVisuals();
	}

	public void AddFill(float delta, Liquid liquid)
	{
		if (delta <= 0f || IsFull || liquid == null) return;

		float prev = fillAmount;
		fillAmount = Mathf.Clamp01(fillAmount + delta);
		float actualAdded = fillAmount - prev;

		if (actualAdded > 0f)
		{
			liquidAmounts.TryGetValue(liquid, out float existing);
			liquidAmounts[liquid] = existing + actualAdded;
			MixColor(liquid.color, actualAdded);
		}

		ApplyVisuals();
	}

	public void ResetGlass()
	{
		fillAmount = 0f;
		totalFillAdded = 0f;
		mixedColor = Color.white;
		liquidAmounts.Clear();
		ApplyVisuals();
	}

	public RecipeScore CheckRecipe(Request request)
	{
		var result = new RecipeScore { ingredients = new List<IngredientScore>() };

		if (request == null || request.recipe == null || request.recipe.ingredients == null || request.recipe.ingredients.Length == 0)
			return result;

		Recipe recipe = request.recipe;
		float targetFill = request.targetFill;

		float matchedTotal = 0f;
		float ingredientScoreSum = 0f;

		foreach (Recipe.Ingredient ing in recipe.ingredients)
		{
			float targetAmount = targetFill * ing.ratio;

			float actualAmount = 0f;
			if (ing.liquid != null)
			{
				liquidAmounts.TryGetValue(ing.liquid, out actualAmount);
			}

			float matched = Mathf.Min(actualAmount, targetAmount);
			matchedTotal += matched;

			float ingScore = targetAmount > 0f ? (matched / targetAmount) * 100f : 0f;
			ingredientScoreSum += ingScore;

			result.ingredients.Add(new IngredientScore
			{
				liquidName = ing.liquid != null ? ing.liquid.liquidName : "None",
				targetAmount = targetAmount,
				actualAmount = actualAmount,
				score = ingScore
			});
		}

		result.ingredientScore = ingredientScoreSum / recipe.ingredients.Length;

		result.purityScore = fillAmount > 0f ? (matchedTotal / fillAmount) * 100f : 0f;

		float fillDeviation = Mathf.Abs(fillAmount - targetFill);
		result.fillScore = targetFill > 0f ? Mathf.Max(0f, 1f - fillDeviation / targetFill) * 100f : 0f;
		result.total = (result.ingredientScore + result.purityScore + result.fillScore) / 3f;

		return result;
	}

	private void MixColor(Color incoming, float weight)
	{
		float Transparency = Mathf.Clamp((mixedColor.a*totalFillAdded + incoming.a*weight)/(weight+totalFillAdded), 0, 1f);
		totalFillAdded += weight;
		Vector4 OurColor = mixedColor;
		Vector4 OtherColor = incoming;
		//We have to convert color to vector4 before being able to convert to vector3
		Vector3 OurColor3 = OurColor;
		Vector3 OtherColor3 = OtherColor;
		Vector3 NewColor = Vector3.Slerp(OurColor, OtherColor, weight/totalFillAdded);
		mixedColor = new Color(NewColor.x, NewColor.y, NewColor.z, Transparency); //Now turn them back into a 4 vector color obj
	}

	private void ApplyVisuals()
	{
		float newScaleY = Mathf.Lerp(minScaleY, maxScaleY, fillAmount);

		Vector3 scale = fillSquare.localScale;
		scale.y = newScaleY;
		fillSquare.localScale = scale;

		Vector3 pos = fillSquare.localPosition;
		pos.y = fillBottomLocalY + newScaleY * 0.5f;
		fillSquare.localPosition = pos;

		if (fillRenderer != null)
		{
			fillRenderer.GetPropertyBlock(propBlock);
			propBlock.SetColor("_Color", mixedColor);
			fillRenderer.SetPropertyBlock(propBlock);
		}
	}

#if UNITY_EDITOR
	void OnDrawGizmosSelected()
	{
		if (fillSquare == null) return;

		float bottomY = fillSquare.localPosition.y - fillSquare.localScale.y * 0.5f;

		Vector2 squareSize = new Vector2(fillSquare.localScale.x, 0f);

		DrawFillGizmo(bottomY + minScaleY, maxScaleY - minScaleY, new Color(0f, 1f, 0.4f, 0.1f), squareSize);
	}

	private void DrawFillGizmo(float bottomLocalY, float scaleY, Color fillColor, Vector2 size)
	{
		float centreLocalY = bottomLocalY + scaleY * 0.5f;
		Vector3 worldCentre = transform.TransformPoint(new Vector3(fillSquare.localPosition.x, centreLocalY, fillSquare.localPosition.z));
		Vector3 worldSize = transform.TransformVector(new Vector3(size.x, scaleY, 0.02f));
		worldSize = new Vector3(Mathf.Abs(worldSize.x), Mathf.Abs(worldSize.y), Mathf.Abs(worldSize.z));

		Gizmos.color = fillColor;
		Gizmos.DrawCube(worldCentre, worldSize);
	}
#endif
}