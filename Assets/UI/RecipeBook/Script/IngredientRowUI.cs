using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientRowUI : MonoBehaviour
{
	public Image dot;
	public TextMeshProUGUI label;
	public Image barFill;

	public TextMeshProUGUI percentText;

	public void Populate(string liquidName, Color color, float ratio)
	{
		dot.color = color;
		label.text = liquidName;

		barFill.color = color;
		barFill.type = Image.Type.Filled;
		barFill.fillMethod = Image.FillMethod.Horizontal;
		barFill.fillOrigin = (int)Image.OriginHorizontal.Left;
		barFill.fillAmount = ratio;

		percentText.text = Mathf.RoundToInt(ratio * 100f) + "%";
	}
}