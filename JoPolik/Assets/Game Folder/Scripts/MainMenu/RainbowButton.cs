using UnityEngine;
using UnityEngine.UI;

public class RainbowButton : MonoBehaviour
{
    private Image buttonImage;
    private float hue = 0f;

    void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    void Update()
    {

        hue += Time.deltaTime * 0.2f;
        if (hue > 1f) hue = 0f;


        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);


        buttonImage.color = rainbowColor;
    }

}