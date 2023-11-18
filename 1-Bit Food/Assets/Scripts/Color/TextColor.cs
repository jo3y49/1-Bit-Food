using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextColor : EditColor {
    private TextMeshProUGUI tm;

    protected override void OnEnable() {
        tm = GetComponent<TextMeshProUGUI>();

        base.OnEnable();
    }


    public override void ChangeColor(Color color)
    {
        tm.color = color;
    }
}