using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightSelected : MonoBehaviour
{
    [SerializeField] private Color HighlightColor = new Color(0f,0.4f,1f,1f);
    [SerializeField] private TextMeshProUGUI TextElement; //We have to change the color of the text inside the button
    private Color OriginalColor;

    //Wanted to use m_fontColor directly but I'm not allowed apparently
    private void Awake()
    {
        OriginalColor = TextElement.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Enter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exit();    
    }

    public void Enter()
    {
        TextElement.color = HighlightColor;
    }

    public void Exit() 
    {
        TextElement.color = OriginalColor;
    }
}
