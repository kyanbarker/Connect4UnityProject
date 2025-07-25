using UnityEngine;

public class Token : MonoBehaviour
{
    public Color Color
    {
        get { return SpriteRenderer.color; }
        set
        {
            Color color = value;
            color.a = Opacity;
            SpriteRenderer.color = color;
        }
    }

    public float Opacity { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Opacity = 1; // 1 is the default value of Opacity
    }
}