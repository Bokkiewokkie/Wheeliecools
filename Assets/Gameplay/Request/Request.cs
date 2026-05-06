using UnityEngine;

[CreateAssetMenu(fileName = "NewRequest", menuName = "Request")]
public class Request : ScriptableObject
{
	public Recipe recipe;

	[Range(0f, 1f)]
	[Tooltip("How full the glass should be (0–1 of its total capacity)")]
	public float targetFill = 0.75f;

	// Glass
	// Additional?

	public static Request GenerateRandom(Recipe[] recipePool, float minFill = 0.5f, float maxFill = 1f, int fillSteps = 4)
	{
		if (recipePool == null || recipePool.Length == 0)
		{
			Debug.LogWarning("Request.GenerateRandom: recipePool is empty.");
			return null;
		}

		var valid = System.Array.FindAll(recipePool, r => r != null);
		if (valid.Length == 0)
		{
			Debug.LogWarning("Request.GenerateRandom: no valid recipes in pool.");
			return null;
		}

		int minStep = Mathf.RoundToInt(Mathf.Clamp01(minFill) * fillSteps);
		int maxStep = Mathf.RoundToInt(Mathf.Clamp01(maxFill) * fillSteps);

		int chosenStep = Random.Range(minStep, maxStep + 1);

		Request request = CreateInstance<Request>();
		request.recipe = valid[Random.Range(0, valid.Length)];
		request.targetFill = chosenStep / (float)fillSteps;

		Debug.Log("New Request:");
		Debug.Log(request.recipe.ToString());
		Debug.Log(request.targetFill.ToString());

		return request;
	}
}