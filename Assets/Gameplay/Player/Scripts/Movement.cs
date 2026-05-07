using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
	// Move your player on the grid, inspect closer if you go to an interactable object!

	[Header("Grid Settings")]
	public float gridSize = 1f;
	public float moveSpeed = 5f;
	public float rotateSpeed = 180f;

	[Header("Camera Inspect Settings")]
	public Camera playerCamera;
	public float inspectForwardOffset = 0.3f;
	public float inspectDownOffset = 0.2f;
	public float inspectLerpSpeed = 4f;

	[Header("Glass Hold Settings")]
	public float holdDistance = 0.55f;
	public float holdMouseRange = 0.15f;
	public float holdLerpSpeed = 18f;
	public LayerMask glassSpotMask = Physics.DefaultRaycastLayers;

	private bool isMoving = false;
	private bool isInspecting = false;

	private Vector3 cameraRestPosition;
	private Vector3 cameraInspectPosition;

	public Glass heldGlass;

	public Key forward = Key.W;
	public Key backward = Key.S;
	public Key left = Key.A;
	public Key right = Key.D;

	private InspectableObject currentInspectable;

	void Start()
	{
		if (playerCamera == null)
			playerCamera = GetComponentInChildren<Camera>();

		cameraRestPosition = playerCamera.transform.localPosition;
		cameraInspectPosition = cameraRestPosition
			+ Vector3.forward * inspectForwardOffset
			+ Vector3.down * inspectDownOffset;
	}

	void Update()
	{
		Vector3 targetCamPos = isInspecting ? cameraInspectPosition : cameraRestPosition;
		playerCamera.transform.localPosition = Vector3.Lerp(
			playerCamera.transform.localPosition,
			targetCamPos,
			Time.deltaTime * inspectLerpSpeed
		);

		if (heldGlass != null)
			UpdateHeldGlassPosition();

		if (isInspecting && Mouse.current.leftButton.wasPressedThisFrame)
			TryClickGlassSpot();

		if (isMoving) return;

		if (Keyboard.current[forward].wasPressedThisFrame)
		{
			TryMoveForward();
		}
		else if (Keyboard.current[backward].wasPressedThisFrame)
		{
			if (isInspecting) StopInspecting();
		}
		else if (Keyboard.current[left].wasPressedThisFrame)
		{
			StartCoroutine(RotateTo(-90f));
		}
		else if (Keyboard.current[right].wasPressedThisFrame)
		{
			StartCoroutine(RotateTo(90f));
		}
	}

	private void StartInspecting(InspectableObject target)
	{
		isInspecting = true;
		currentInspectable = target;
		target?.StartInspect();
	}

	private void StopInspecting()
	{
		isInspecting = false;
		currentInspectable?.StopInspect();
		currentInspectable = null;
	}

	private void TryClickGlassSpot()
	{
		Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

		if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, glassSpotMask))
			return;

		GlassSpot spot = hit.collider.GetComponent<GlassSpot>();
		if (spot == null) return;

		if (heldGlass == null)
		{
			if (spot.HasGlass)
			{
				heldGlass = spot.TakeGlass();

				// Disable collider while carrying so it doesn't block raycasts
				Collider col = heldGlass.GetComponent<Collider>();
				if (col != null) col.enabled = false;

				Debug.Log("Picked up: " + heldGlass.name);
			}
		}
		else
		{
			if (spot.IsEmpty)
			{
				if (spot.PlaceGlass(heldGlass))
				{
					Debug.Log("Placed glass on: " + spot.name);
					heldGlass = null;
				}
			}
			else
			{
				Debug.Log("Spot already occupied!");
			}
		}
	}

	private void UpdateHeldGlassPosition()
	{
		Ray mouseRay = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

		Plane holdPlane = new Plane(
			-playerCamera.transform.forward,
			playerCamera.transform.position + playerCamera.transform.forward * holdDistance
		);

		Vector3 targetPos;
		if (holdPlane.Raycast(mouseRay, out float enter))
		{
			Vector3 hit = mouseRay.GetPoint(enter);
			Vector3 centre = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
			Vector3 offset = Vector3.ClampMagnitude(hit - centre, holdMouseRange);
			targetPos = centre + offset;
		}
		else
		{
			targetPos = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
		}

		if (isMoving)
		{
			heldGlass.transform.position = targetPos;
		}
		else
		{
			heldGlass.transform.position = Vector3.Lerp(heldGlass.transform.position, targetPos, Time.deltaTime * holdLerpSpeed);
		}

		// Keep the glass upright and facing the camera
		heldGlass.transform.rotation = Quaternion.LookRotation(
			playerCamera.transform.forward, Vector3.up);
	}

	void TryMoveForward()
	{
		RaycastHit hit;
		bool blocked = Physics.Raycast(transform.position, transform.forward, out hit, gridSize);

		if (blocked)
		{
			if (hit.collider.CompareTag("Interactable"))
			{
				InspectableObject inspectable = hit.collider.GetComponent<InspectableObject>();
				StartInspecting(inspectable);
				Debug.Log("Inspecting: " + hit.collider.name);
			}
			else
			{
				StopInspecting(); 
				Debug.Log("Blocked by obstacle!");
			}
			return;
		}

		// Moving away from whatever we were looking at, stop inspecting
		StopInspecting();

		Vector3 targetPosition = transform.position + transform.forward * gridSize;
		StartCoroutine(MoveTo(targetPosition));
	}

	IEnumerator MoveTo(Vector3 targetPosition)
	{
		isMoving = true;

		while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
		{
			transform.position = Vector3.MoveTowards(
				transform.position,
				targetPosition,
				moveSpeed * Time.deltaTime
			);
			yield return null;
		}

		transform.position = targetPosition;
		isMoving = false;
	}

	IEnumerator RotateTo(float angleDelta)
	{
		isMoving = true;
		StopInspecting(); // ?? CHANGED (was: isInspecting = false)

		Quaternion startRotation = transform.rotation;
		Quaternion targetRotation = Quaternion.Euler(
			transform.eulerAngles + new Vector3(0f, angleDelta, 0f)
		);

		float elapsed = 0f;
		float duration = Mathf.Abs(angleDelta) / rotateSpeed;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
			yield return null;
		}

		transform.rotation = targetRotation;
		isMoving = false;
	}
}