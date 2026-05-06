using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GlassSpot : MonoBehaviour
{
	[Header("References")]
	[Tooltip("BOTTOM of the glass")]
	public Transform glassAnchor;

	[Header("Starting Glass")]
	public Glass startingGlassPrefab;

	private Glass currentGlass;

	public bool IsEmpty => currentGlass == null;
	public bool HasGlass => currentGlass != null;
	public Glass CurrentGlass => currentGlass;

	void Awake()
	{
		// Register any glass already placed in the scene as a child of the anchor
		currentGlass = AnchorOrSelf().GetComponentInChildren<Glass>();
	}

	void Start()
	{
		if (IsEmpty && startingGlassPrefab != null)
		{
			PlaceGlass(Instantiate(startingGlassPrefab));
		}
	}

	public Glass TakeGlass()
	{
		if (currentGlass == null) return null;

		Glass taken = currentGlass;
		currentGlass = null;
		taken.transform.SetParent(null);
		return taken;
	}

	public bool PlaceGlass(Glass glass)
	{
		if (!IsEmpty || glass == null) return false;

		float halfHeight = GetHalfHeight(glass);

		currentGlass = glass;
		Transform anchor = AnchorOrSelf();
		glass.transform.SetParent(anchor);

		// Lift so the bottom edge of the sprite sits on the anchor point
		glass.transform.localPosition = Vector3.up * halfHeight;
		glass.transform.localRotation = Quaternion.identity;

		return true;
	}
	private Transform AnchorOrSelf() => glassAnchor != null ? glassAnchor : transform;

	private static float GetHalfHeight(Glass glass)
	{
		SpriteRenderer sr = glass.GetComponentInChildren<SpriteRenderer>();
		if (sr != null) return sr.bounds.extents.y;

		// Fallback if no SpriteRenderer is found
		return glass.transform.localScale.y * 0.5f;
	}

#if UNITY_EDITOR
	void OnDrawGizmosSelected()
	{
		Transform anchor = AnchorOrSelf();
		Gizmos.color = currentGlass != null
			? new Color(0f, 1f, 0.3f, 0.35f)
			: new Color(1f, 1f, 0f, 0.2f);
		Gizmos.matrix = anchor.localToWorldMatrix;
		Gizmos.DrawCube(new Vector3(0f, 0.175f, 0f), new Vector3(0.25f, 0.35f, 0.25f));
		Gizmos.matrix = Matrix4x4.identity;
	}
#endif
}
