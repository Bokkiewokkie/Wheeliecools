using UnityEngine;
using UnityEngine.InputSystem;

public class Tapper : MonoBehaviour
{
	// Fill up glasses with a liquid, currently very very manual with Keys

	public GlassSpot glassSpot;

	public Liquid[] liquids = new Liquid[0];
	private int liquidIndex = 0;

	[Header("Keybindings")]
	public Key fillKey = Key.Space;
	public Key nextColour = Key.E;
	public Key prevColour = Key.Q;
	public Key resetGlass = Key.R;

	public Glass SelectedGlass => glassSpot?.CurrentGlass;
	public Color ActiveColour => (liquids != null && liquids.Length > 0) ? liquids[liquidIndex].color : Color.white;
	public Liquid ActiveLiquid => (liquids != null && liquids.Length > 0) ? liquids[liquidIndex] : null;

	void Update()
	{
		if (glassSpot == null) return;

		HandleLiquidSelection();
		HandleFilling();
		HandleReset();
	}

	private void HandleLiquidSelection()
	{
		if (liquids == null || liquids.Length == 0) return;

		if (Keyboard.current[nextColour].wasPressedThisFrame)
		{
			liquidIndex = (liquidIndex + 1) % liquids.Length;
		}
		if (Keyboard.current[prevColour].wasPressedThisFrame)
		{
			liquidIndex = (liquidIndex - 1 + liquids.Length) % liquids.Length;
		}
	}

	private void HandleFilling()
	{
		if (!Keyboard.current[fillKey].isPressed) return;

		Glass target = SelectedGlass;
		if (target == null || target.IsFull) return;

		target.AddFill(target.FillDelta, ActiveLiquid);
	}

	private void HandleReset()
	{
		if (Keyboard.current[resetGlass].wasPressedThisFrame)
			SelectedGlass?.ResetGlass();
	}

	void OnDrawGizmos()
	{
		if (glassSpot.CurrentGlass == null) return;
		Gizmos.color = (liquids != null && liquids.Length > 0)
				? liquids[liquidIndex].color
				: Color.gray;
		Gizmos.DrawWireSphere(glassSpot.CurrentGlass.transform.position, 0.15f);
	}
}