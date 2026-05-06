using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image Filling;

    public void OnSpawn(Color color)
    {
        Filling.color = color;
    }
}
