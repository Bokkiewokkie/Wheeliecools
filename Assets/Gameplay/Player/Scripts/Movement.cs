using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridPlayerController : MonoBehaviour
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

	private bool isMoving = false;
	private bool isInspecting = false;

	private Vector3 cameraRestPosition;
	private Vector3 cameraInspectPosition;

	public Key forward = Key.W;
	public Key backward = Key.S;
	public Key left = Key.A;
	public Key right = Key.D;

	void Start()
	{
		if (playerCamera == null)
			playerCamera = GetComponentInChildren<Camera>();

		cameraRestPosition = playerCamera.transform.localPosition;
		cameraInspectPosition = cameraRestPosition + Vector3.forward * inspectForwardOffset + Vector3.down * inspectDownOffset;
	}

	void Update()
	{
		Vector3 targetCamPos = isInspecting ? cameraInspectPosition : cameraRestPosition;
		playerCamera.transform.localPosition = Vector3.Lerp(
			playerCamera.transform.localPosition,
			targetCamPos,
			Time.deltaTime * inspectLerpSpeed
		);

		if (isMoving) return;

		if (Keyboard.current[forward].wasPressedThisFrame)
		{
			TryMoveForward();
		}
		if (Keyboard.current[backward].wasPressedThisFrame)
		{
			if (isInspecting) isInspecting = false;
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

	void TryMoveForward()
	{
		RaycastHit hit;
		bool blocked = Physics.Raycast(transform.position, transform.forward, out hit, gridSize);

		if (blocked)
		{
			if (hit.collider.CompareTag("Interactable"))
			{
				isInspecting = true;
				Debug.Log("Inspecting: " + hit.collider.name);
			}
			else
			{
				isInspecting = false;
				Debug.Log("Blocked by obstacle!");
			}
			return;
		}

		// Moving away from whatever we were looking at, stop inspecting
		isInspecting = false;

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

		isInspecting = false;

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