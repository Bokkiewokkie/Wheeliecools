using UnityEngine;
using UnityEngine.Events;

public class InspectableObject : MonoBehaviour
{

	//Calls events when the player inspects

	[Header("Inspect Events")]
	public UnityEvent OnInspectStart;
	public UnityEvent OnInspectStop;

	public void StartInspect()
	{
		OnInspectStart?.Invoke();
	}

	public void StopInspect()
	{
		OnInspectStop?.Invoke();
	}
}