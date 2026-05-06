using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
	public List<Request> Requests = new List<Request>();

	public float minFill = 0.25f;
	public float maxFill = 1f;
	public int fillSteps = 4;

	// Singleton design pattern, only 1 RequestManager can exist at a time.
	public static RequestManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			Debug.Log("RequestManager already exists");
		}
		else
		{
			Instance = this;
		}
	}

	public void GenerateRequest()
	{
		Request random = Request.GenerateRandom(RecipeManager.Instance.Recipes, minFill, maxFill);
		Requests.Add(random);
	}
}
#if UNITY_EDITOR
[CustomEditor(typeof(RequestManager))]
public class RequestManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		RequestManager manager = (RequestManager)target;

		GUILayout.Space(10);

		if (GUILayout.Button("Generate Request"))
		{
			manager.GenerateRequest();
			EditorUtility.SetDirty(manager);
		}
	}
}
#endif