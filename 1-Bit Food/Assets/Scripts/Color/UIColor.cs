using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIColor : EditColor {
    private Image image;

    protected override void Start() {
        image = GetComponent<Image>();

        base.Start();
    }


    public override void ChangeColor(Color color)
    {
        image.color = color;
    }
}