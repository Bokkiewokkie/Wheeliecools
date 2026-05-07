using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookUI : MonoBehaviour
{
	public RectTransform cardContainer;
	public RecipeCardUI recipeCardPrefab;

	private void Start()
	{
		BuildRecipeBook();
	}

	public void BuildRecipeBook()
	{
		if (RecipeManager.Instance == null)
		{
			Debug.LogError("RecipeBookUI: no RecipeManager found in scene.");
			return;
		}

		ClearCards();

		int recipeCount = 0;
		float cardSizeStart = 230;
		float cardSize = 230;
		GridLayoutGroup group = cardContainer.GetComponent<GridLayoutGroup>();

		foreach (Recipe recipe in RecipeManager.Instance.Recipes)
		{
			float size = 0;
			RecipeCardUI card = Instantiate(recipeCardPrefab, cardContainer);
			size = cardSizeStart + (recipe.ingredients.Length*30);

			Debug.Log((recipe.ingredients.Length * 30));
			Debug.Log(size);

			if (size > cardSize) cardSize = size;	

			card.Populate(recipe);
			recipeCount++;
		}

		group.cellSize = new Vector2(group.cellSize.x, cardSize);

		LayoutRebuilder.ForceRebuildLayoutImmediate(cardContainer);

		float sizey = (group.cellSize.y + group.padding.bottom) * (recipeCount/3);
		RectTransform rt  = cardContainer.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(rt.sizeDelta.x, sizey);
	}

	private void ClearCards()
	{
		for (int i = cardContainer.childCount - 1; i >= 0; i--)
			Destroy(cardContainer.GetChild(i).gameObject);
	}
}