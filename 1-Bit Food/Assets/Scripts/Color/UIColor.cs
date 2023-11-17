using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIColor : EditColor {
    protected Image image;

    protected override void OnEnable() {
        image = GetComponent<Image>();

        base.OnEnable();
    }


    public override void ChangeColor(Color color)
    {
        image.color = color;
    }
}