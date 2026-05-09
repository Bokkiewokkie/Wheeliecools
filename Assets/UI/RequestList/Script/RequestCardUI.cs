using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestCardUI : MonoBehaviour
{
	[Header("Text Fields")]
	public TextMeshProUGUI recipeNameText;
	public TextMeshProUGUI fillText;
	public TextMeshProUGUI glassTypeText;

	[Header("Timer")]
	public Image timerBar;

	public Color timerFullColour = Color.green;
	public Color timerEmptyColour = Color.red;

	public Request Request { get; private set; }

	private float _duration;
	private Coroutine _timerCoroutine;

	public void Initialize(Request request, float duration)
	{
		Request = request;
		_duration = duration;

		PopulateText(request);
		ResetTimerBar();

		if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
		_timerCoroutine = StartCoroutine(TimerRoutine());
	}

	private void PopulateText(Request request)
	{
		if (recipeNameText != null)
			recipeNameText.text = request.recipe != null ? request.recipe.recipeName : "???";

		if (fillText != null)
			fillText.text = $"Fill: {request.targetFill * 100f:0}%";

		if (glassTypeText != null)
			glassTypeText.text = $"Glass: {request.glassType}";
	}

	private void ResetTimerBar()
	{
		if (timerBar == null) return;
		timerBar.fillAmount = 1f;
		timerBar.color = timerFullColour;
	}

	private IEnumerator TimerRoutine()
	{
		float elapsed = 0f;

		while (elapsed < _duration)
		{
			elapsed += Time.deltaTime;

			float t = Mathf.Clamp01(elapsed / _duration);

			if (timerBar != null)
			{
				timerBar.fillAmount = 1f - t;
				timerBar.color = Color.Lerp(timerFullColour, timerEmptyColour, t);
			}

			yield return null;
		}

		if (timerBar != null)
		{
			timerBar.fillAmount = 0f;
			timerBar.color = timerEmptyColour;
		}
	}

	private void OnDestroy()
	{
		if (_timerCoroutine != null) StopCoroutine(_timerCoroutine);
	}
}