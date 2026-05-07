using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeCardUI : MonoBehaviour
{
	public RectTransform colourSquare;
	public TextMeshProUGUI recipeName;
	public RectTransform ingredientList;

	[Header("Prefabs")]
	public Image colorBandPrefab;
	public IngredientRowUI ingredientRowPrefab;

	public void Populate(Recipe recipe)
	{
		recipeName.text = recipe.recipeName;

		ClearChildren(colourSquare);
		ClearChildren(ingredientList);

		foreach (Recipe.Ingredient ingredient in recipe.ingredients)
		{
			Image band = Instantiate(colorBandPrefab, colourSquare);
			band.color = ingredient.liquid.color;

			LayoutElement le = band.GetComponent<LayoutElement>();
			if (le == null) le = band.gameObject.AddComponent<LayoutElement>();
			le.flexibleHeight = ingredient.ratio;
			le.minHeight = 0f;
			le.preferredHeight = -1f;

			IngredientRowUI row = Instantiate(ingredientRowPrefab, ingredientList);
			row.Populate(ingredient.liquid.liquidName, ingredient.liquid.color, ingredient.ratio);
		}
	}

	private static void ClearChildren(Transform parent)
	{
		for (int i = parent.childCount - 1; i >= 0; i--)
			Destroy(parent.GetChild(i).gameObject);
	}
}