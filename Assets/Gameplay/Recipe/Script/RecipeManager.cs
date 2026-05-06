using UnityEngine;

public class RecipeManager : MonoBehaviour
{
	public Recipe[] Recipes;

	// Singleton design pattern, only 1 RecipeManager can exist at a time.
	public static RecipeManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			Debug.Log("RecipeManager already exists");
		}
		else
		{
			Instance = this;
		}
	}
}
