using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewLiquid", menuName = "Drink/Liquid")]
public class Liquid : ScriptableObject
{
	// dinking my oiter
	public string liquidName = "Unknown";
	public Color color = Color.white;
	public float price = 1f; //per liter, in the 1920s of course

#if UNITY_EDITOR
	private void OnValidate()
	{
		// Only auto-fill if it's still default or empty
		if (string.IsNullOrEmpty(liquidName) || liquidName == "Unknown")
		{
			string path = AssetDatabase.GetAssetPath(this);
			if (!string.IsNullOrEmpty(path))
			{
				liquidName = Path.GetFileNameWithoutExtension(path);
				EditorUtility.SetDirty(this);
			}
		}
	}
#endif
}