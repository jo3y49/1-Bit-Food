using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ButtonColor : UIColor
{
    private TextMeshProUGUI tm;

    protected override void OnEnable()
    {
        tm = GetComponentInChildren<TextMeshProUGUI>();

        base.OnEnable();
    }

    public override void ChangeColor(Color color)
    {
        base.ChangeColor(color);

        if (tm == null) return;

        if (gameColor == ColorSwitcher.GameColor.Bright) tm.color = ColorSwitcher.instance.Dark;

        else tm.color = ColorSwitcher.instance.Bright;
    }
}