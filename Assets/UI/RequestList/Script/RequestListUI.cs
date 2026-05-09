using System.Collections.Generic;
using UnityEngine;

public class RequestListUI : MonoBehaviour
{
	public RequestCardUI cardPrefab;

	public Transform cardContainer;

	[Header("Settings")]
	public float requestDuration = 30f;

	private readonly Dictionary<Request, RequestCardUI> _cards = new();

	private void Start()
	{
		BuildRequestList();
	}

	public void BuildRequestList()
	{
		foreach (var card in _cards.Values)
			if (card != null) Destroy(card.gameObject);
		_cards.Clear();

		if (RequestManager.Instance == null)
		{
			return;
		}

		foreach (Request request in RequestManager.Instance.Requests)
			AddCard(request);
	}

	public void UpdateRequestList()
	{
		Debug.Log("Update List");

		if (RequestManager.Instance == null) return;

		var currentRequests = new HashSet<Request>(RequestManager.Instance.Requests);

		// Remove cards whose requests are gone
		var toRemove = new List<Request>();
		foreach (var kvp in _cards)
			if (!currentRequests.Contains(kvp.Key))
				toRemove.Add(kvp.Key);

		foreach (var req in toRemove)
		{
			if (_cards[req] != null) Destroy(_cards[req].gameObject);
			_cards.Remove(req);
		}

		// Add cards for requests that don't have one yet
		foreach (var req in currentRequests)
			if (!_cards.ContainsKey(req))
				AddCard(req);
	}

	private void AddCard(Request request)
	{
		Debug.Log("Add Card");

		if (cardPrefab == null)
		{
			return;
		}

		if (cardContainer == null)
		{
			return;
		}

		RequestCardUI card = Instantiate(cardPrefab, cardContainer);
		card.Initialize(request, requestDuration);
		_cards[request] = card;
	}
}