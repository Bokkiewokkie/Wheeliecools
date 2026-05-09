using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Customer : MonoBehaviour
{
	[Header("Request Timing")]
	public float requestDuration = 30f;

	[Header("Double Request Settings")]
	public int requestsBeforeDoubleUnlocks = 3;

	[Range(0f, 1f)]
	public float doubleRequestChance = 0.35f;
	public float doubleRequestCooldown = 60f;

	[Header("Events")]
	public UnityEvent<Request> OnRequestExpired;
	public UnityEvent<Request> OnRequestMade;

	private int _fulfilledCount = 0;
	private bool _doubleRequestOnCooldown = false;

	private readonly Dictionary<Request, Coroutine> _activeTimers = new();

	void Start()
	{
		PlaceOrder();
	}

	public void PlaceOrder()
	{
		bool makeDouble = !_doubleRequestOnCooldown && _fulfilledCount >= requestsBeforeDoubleUnlocks && Random.value < doubleRequestChance;

		MakeRequest();

		if (makeDouble)
		{
			MakeRequest();
			StartCoroutine(DoubleRequestCooldownRoutine());
		}
	}

	public void NotifyRequestFulfilled(Request request)
	{
		CancelTimer(request);
		_fulfilledCount++;
		PlaceOrder();
	}

	private void MakeRequest()
	{
		Request request = RequestManager.Instance?.GenerateRequest();
		if (request == null)
		{
			Debug.LogWarning("RequestManager returned null.");
			return;
		}

		OnRequestMade?.Invoke(request);

		Coroutine timer = StartCoroutine(RequestTimerRoutine(request));
		_activeTimers[request] = timer;
	}

	private void CancelTimer(Request request)
	{
		if (_activeTimers.TryGetValue(request, out Coroutine timer))
		{
			if (timer != null) StopCoroutine(timer);
			_activeTimers.Remove(request);
		}
	}

	private IEnumerator RequestTimerRoutine(Request request)
	{
		float elapsed = 0f;

		while (elapsed < requestDuration)
		{
			elapsed += Time.deltaTime;
			yield return null;
		}

		// Timer ran out � clean up and notify listeners.
		_activeTimers.Remove(request);
		OnRequestExpired?.Invoke(request);
		RequestManager.Instance?.DestroyRequest(request);
		PlaceOrder(); //Ask for something else
	}

	private IEnumerator DoubleRequestCooldownRoutine()
	{
		_doubleRequestOnCooldown = true;

		yield return new WaitForSeconds(doubleRequestCooldown);

		_doubleRequestOnCooldown = false;
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
		_activeTimers.Clear();
	}
}