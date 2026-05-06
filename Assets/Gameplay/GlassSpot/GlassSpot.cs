using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GlassSpot : MonoBehaviour
{
	public Transform glassAnchor;

	private Glass currentGlass;

	public bool IsEmpty => currentGlass == null;
	public bool HasGlass => currentGlass != null;
	public Glass CurrentGlass => currentGlass;

	void Awake()
	{
		Transform anchor = glassAnchor != null ? glassAnchor : transform;
		currentGlass = anchor.GetComponentInChildren<Glass>();
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

		currentGlass = glass;

		Transform anchor = glassAnchor != null ? glassAnchor : transform;
		glass.transform.SetParent(anchor);
		glass.transform.localPosition = Vector3.zero;
		glass.transform.localRotation = Quaternion.identity;

		Collider col = glass.GetComponent<Collider>();
		if (col != null) col.enabled = true;

		return true;
	}

#if UNITY_EDITOR
	void OnDrawGizmosSelected()
	{
		Transform anchor = glassAnchor != null ? glassAnchor : transform;
		Gizmos.color = currentGlass != null
			? new Color(0f, 1f, 0.3f, 0.35f)
			: new Color(1f, 1f, 0f, 0.2f);
		Gizmos.matrix = anchor.localToWorldMatrix;
		Gizmos.DrawCube(Vector3.zero, new Vector3(0.25f, 0.35f, 0.25f));
		Gizmos.matrix = Matrix4x4.identity;
	}
#endif
}